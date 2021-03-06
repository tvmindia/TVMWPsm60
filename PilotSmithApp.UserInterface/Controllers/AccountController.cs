﻿using AutoMapper;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PilotSmithApp.UserInterface.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class AccountController : Controller
    {
        Const _const = new Const();
        IUserBusiness _userBusiness;
        
        Guid AppID = Guid.Parse(ConfigurationManager.AppSettings["ApplicationID"]);
        public AccountController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }
        // GET: Account
        public ActionResult Index() 
        {
            return View();
        }

        #region Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel loginvm)
        {
            UserViewModel uservm = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    loginvm.IsFailure = true;
                    loginvm.Message = _const.LoginFailed;
                    return View("Index", loginvm);
                }
                uservm = Mapper.Map<User, UserViewModel>(_userBusiness.CheckUserCredentials(Mapper.Map<LoginViewModel, User>(loginvm)));
                if (uservm != null)
                {
                    if (uservm.RoleList == null || uservm.RoleList.Count == 0 && string.IsNullOrEmpty(uservm.RoleIDCSV))
                    {
                        loginvm.IsFailure = true;
                        loginvm.Message = _const.LoginFailedNoRoles;
                        return View("Index", loginvm);
                    }
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, uservm.UserName, DateTime.Now, DateTime.Now.AddHours(24), true, uservm.RoleCSV);
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
                    //session setting
                    UA ua = new UA();
                    ua.UserName = uservm.LoginName;
                    ua.AppID = AppID;
                    Session.Add("TvmValid", ua);
                    Session.Add("UserRights", _userBusiness.GetAllAccess(uservm.LoginName));                    
                    return RedirectToLocal();
                }
                else
                {
                    loginvm.IsFailure = true;
                    loginvm.Message = _const.LoginFailed;
                    return View("Index", loginvm);
                }



            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion UserInsertUpdate

        #region Logout
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Remove("TvmValid");
                Session.Remove("UserRights");
                Session.Remove("AppUA");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToLogin();
        }

        #endregion Logout
        private ActionResult RedirectToLocal()
        {
            return RedirectToAction("Index", "DashBoard");
        }
        private ActionResult RedirectToLogin()
        {
            return RedirectToAction("Index", "Account");
        }
        [HttpGet]
        public ActionResult NotAuthorized()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Down()
        {
            return View();
        }
        [HttpPost]
        public string AreyouAlive()
        {
            string result = "";
            try
            {
                UA uaObj = null;
                if ((System.Web.HttpContext.Current.Session != null) && (System.Web.HttpContext.Current.Session["TvmValid"] != null))
                {
                    uaObj = (UA)System.Web.HttpContext.Current.Session["TvmValid"];
                    result = "alive";
                }
                else
                {
                    result = "dead";
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }

            return JsonConvert.SerializeObject(new { Result = "OK", Record = result });
        }
    }
}