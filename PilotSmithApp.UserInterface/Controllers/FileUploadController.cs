﻿using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.BusinessService.Contracts;
using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class FileUploadController : Controller
    {
        IFileUploadBusiness _fileUploadBusiness;
        PSASysCommon _pSASysCommon = new PSASysCommon();
        public FileUploadController(IFileUploadBusiness fileUploadBusiness, IApproverBusiness approverBusiness)
        {
            _fileUploadBusiness = fileUploadBusiness;
        }
        // GET: FileUplad
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                Guid FileID = Guid.NewGuid();
                FileUpload _fileObj = new FileUpload();
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        FileUpload fileuploadObj = new FileUpload();
                        HttpPostedFileBase file = files[i];
                        string fname;
                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        fileuploadObj.AttachmentURL = "/Content/Uploads/" + fname;
                        fileuploadObj.FileSize = file.ContentLength.ToString();
                        fileuploadObj.FileType = file.ContentType;
                        fileuploadObj.FileName = fname;
                        fileuploadObj.ParentID = Request["ParentID"].ToString() != "" ? Guid.Parse(Request["ParentID"].ToString()) : FileID;
                        fileuploadObj.ParentType = Request["ParentID"].ToString() != "" ? Request["ParentType"] : null;
                        fileuploadObj.IsDocumentApprover= Request["IsDocumentApprover"].ToString() == "True" ? true : false;
                        fileuploadObj.PSASysCommon = new PSASysCommon();
                        AppUA _appUA = Session["AppUA"] as AppUA;
                        fileuploadObj.PSASysCommon.CreatedBy = _appUA.UserName;
                        //fileuploadObj.PSASysCommon.CreatedDate = _appUA.LoginDateTime;
                        fileuploadObj.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                        fileuploadObj.PSASysCommon.UpdatedBy = _appUA.UserName;
                        fileuploadObj.PSASysCommon.UpdatedDate = _appUA.LoginDateTime;
                        _fileObj = _fileUploadBusiness.InsertAttachment(fileuploadObj);
                        fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), fname);
                        file.SaveAs(fname);
                    }
                    if (_fileObj.ParentID == FileID)
                    {
                        return Json(new { Result = "OK", Message = "File Uploaded Successfully!", Records = _fileObj });
                    }
                    else
                    {
                        // _fileObj.ParentID = Guid.Empty;
                        return Json(new { Result = "OK", Message = "File Uploaded Successfully!", Records = _fileObj });
                    }

                }
                catch (Exception ex)
                {
                    return Json(new { Result = "Error", Message = "Error occurred. Error details: " + ex.Message });
                }
            }
            else
            {
                return Json(new { Result = "Error", Message = "No files selected." });
            }
        }
        [HttpGet]
        public string GetAttachments(string ID)
        {
            try
            {
                List<FileUpload> AttachmentList = new List<FileUpload>();
                AttachmentList = _fileUploadBusiness.GetAttachments(Guid.Parse(ID));
                AppUA appUA = Session["AppUA"] as AppUA;
                if (AttachmentList != null)
                    foreach (FileUpload item in AttachmentList)
                    {
                        item.IsDocLocked = item.DocumentOwners.Contains(appUA.UserName);
                        string[] owner = ((!string.IsNullOrEmpty(ConfigurationManager.AppSettings["owners"])) ? ConfigurationManager.AppSettings["owners"].Split(',') : null);
                        if ((owner != null) && (owner.Any(item.ParentType.Equals)))
                        {
                            item.IsDocLocked = true;
                        }
                        if(item.ParentType=="Quotation"||item.ParentType== "SaleOrder"||item.ParentType== "ProductionOrder"|| item.ParentType == "ProformaInvoice")
                        {
                            switch (item.ParentType)
                            {
                                case "Quotation":
                                case "SaleOrder":
                                case "ProformaInvoice":
                                case "ProductionOrder":
                                    //item.IsDocumentApprover= _approverBusiness.CheckIsDocumentOwner("QUO", appUA.UserName);
                                    item.IsDocumentApprover = true;
                                    break;
                            }
                        }
                    }
                return JsonConvert.SerializeObject(new { Result = "OK", Records = AttachmentList });
            }
            catch (Exception ex)
            {
                //AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpGet]
        public FileResult DownloadFile(string token)
        {
            if (token != "")
            {
                string[] Filess = token.Split('/');
                string filename = Server.MapPath(token);
                //string contentType = "application/image";
                string contentType = MimeMapping.GetMimeMapping(Filess[3]);
                //Parameters to file are
                //1. The File Path on the File Server
                //2. The content type MIME type
                //3. The parameter for the file save by the browser
                return File(filename, contentType, Filess[3]);
            }
            return null;
        }
        [HttpGet]
        public string DeleteFile(string id)
        {
            AppConst c = new AppConst();
            object result = null;
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                result = _fileUploadBusiness.DeleteFile(Guid.Parse(id),appUA.UserName, _pSASysCommon.GetCurrentDateTime());
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess, Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
    }
}