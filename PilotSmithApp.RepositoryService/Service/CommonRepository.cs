﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using PilotSmithApp.RepositoryService.Contract;

namespace PilotSmithApp.RepositoryService.Service
{
    public class CommonRepository : ICommonRepository
    {
        private IDatabaseFactory _databaseFactory;
        public CommonRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        //send Message

        #region messageSending


        public string SendMessage(string Msg, string MobileNos, string provider = "txtlocal", string type = "OTP")
        {
            string result = null;
            

                string[] IndividualMsgs = Msg.Split('|');
                string[] IndividualMobileNos = MobileNos.Split('|');
                foreach (var msg in IndividualMsgs) //msg is individual message
                {
                    
                    foreach (var Num in IndividualMobileNos) //Num is individual Number
                    {
                        if (Num != string.Empty)
                        {
                            if (msg != string.Empty)
                            {
                                String message = HttpUtility.UrlEncode(msg);
                                #region textlocal
                                //--------------------------------------------------------------------------------------------------
                                if (provider == "txtlocal")
                                {
                                    using (var wb = new System.Net.WebClient())
                                    {
                                        byte[] response = wb.UploadValues("https://api.textlocal.in/send/", "POST", new NameValueCollection()
                                {
                                {"username" , "suvaneeth@gmail.com"},
                                {"hash" , "0f6f640793dfe7fd4c75ef55b57c2f841986f71e8c52fbdea5f6cb52cc723603"},
                                { "apiKey","dSGmbXNsOJU-OZI40tsiF6tEwF6fCgEVq3uZ9lpd56"},
                                {"sender" , "TXTLCL"},
                                {"numbers" , Num},
                                {"message" , message}
                                });
                                        result = System.Text.Encoding.UTF8.GetString(response);

                                    }
                                }
                                #endregion

                                #region smshorizon
                                //-------------------------------------------------------------------------------------------------------
                                else if (provider == "smshorizon")
                                {

                                    using (var wb = new System.Net.WebClient())
                                    {
                                        byte[] response = wb.UploadValues("http://smshorizon.co.in/api/sendsms.php", "POST", new NameValueCollection()
                                {
                                {"user" , "suvaneeth"},
                                {"apikey" , "Ge0hv03z2WvwlBOTK3B0"},
                                {"mobile" , Num},
                                {"message" , msg},
                                        { "senderid","MYTEXT"},
                                { "type","txt"}
                                });
                                        result = System.Text.Encoding.UTF8.GetString(response);

                                    }

                                }

                                #endregion

                                #region 2factor
                                //-----------------------------------------------------------------------------------------------------------
                                else if (provider == "2factor" && type == "OTP")
                                {


                                



                                    string otpTemplate = System.Web.Configuration.WebConfigurationManager.AppSettings["2factorOTP"];

                                    if (!String.IsNullOrEmpty(otpTemplate))
                                    {

                                        String url = "https://2factor.in/API/V1/bddc3759-107a-11e7-9462-00163ef91450/SMS/" + MobileNos + "/" + msg + "/" + otpTemplate + "";

                                        HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(url);
                                        httpWReq.Method = "POST";
                                        httpWReq.ContentType = "application/x-www-form-urlencoded";
                                        HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                                        string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


                                    }
                                }
                                #endregion
                               //-------------------------------------------------------------------------------------------------------------
                            
                            }

                        }
                    }

                }
            return result;

        }
        #endregion
    }

}
