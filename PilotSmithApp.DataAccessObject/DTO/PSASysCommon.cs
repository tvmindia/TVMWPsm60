using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class PSASysCommon
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateString { get; set; }        
        public DateTime GetCurrentDateTime()
        {
            string tz = System.Web.Configuration.WebConfigurationManager.AppSettings["TimeZone"];
            DateTime DateNow = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            return (TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateNow, tz));
        }
        public string NumberToWords(double number)
        {
            string[] numbersArr = new string[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] tensArr = new string[] { "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninty" };
            string[] suffixesArr = new string[] { "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion", "Sextillion", "Septillion", "Octillion", "Nonillion", "Decillion", "Undecillion", "Duodecillion", "Tredecillion", "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septdecillion", "Octodecillion", "Novemdecillion", "Vigintillion" };
            string words = "";

            bool tens = false;

            if (number < 0)
            {
                words += "negative ";
                number *= -1;
            }

            int power = (suffixesArr.Length + 1) * 3;

            while (power > 3)
            {
                double pow = Math.Pow(10, power);
                if (number >= pow)
                {
                    if (number % pow > 0)
                    {
                        words += NumberToWords(Math.Floor(number / pow)) + " " + suffixesArr[(power / 3) - 1] + ", ";
                    }
                    else if (number % pow == 0)
                    {
                        words += NumberToWords(Math.Floor(number / pow)) + " " + suffixesArr[(power / 3) - 1];
                    }
                    number %= pow;
                }
                power -= 3;
            }
            if (number >= 1000)
            {
                if (number % 1000 > 0) words += NumberToWords(Math.Floor(number / 1000)) + " Thousand, ";
                else words += NumberToWords(Math.Floor(number / 1000)) + " Thousand";
                number %= 1000;
            }
            if (0 <= number && number <= 999)
            {
                if ((int)number / 100 > 0)
                {
                    words += NumberToWords(Math.Floor(number / 100)) + " Hundred";
                    number %= 100;
                }
                if ((int)number / 10 > 1)
                {
                    if (words != "")
                        words += " and ";
                    words += tensArr[(int)number / 10 - 2];
                    tens = true;
                    number %= 10;
                }

                if (number < 20 && number > 0)
                {
                    if (words != "" && tens == false)
                        words += " and ";
                    words += (tens ? "-" + numbersArr[(int)number - 1] : numbersArr[(int)number - 1]);
                    number -= Math.Floor(number);
                }
            }

            return words;
        }
    }

    public class AppUA
    {
        public string UserName { get; set; }
        public DateTime LoginDateTime { get; set; }
        public Guid AppID { get; set; }
        public string RolesCSV { get; set; }
    }

    public class Settings
    {

        public string DateFormat = "dd-MMM-yyyy";
    }


    public class AppConst
    {
        #region Messages

        private List<AppConstMessage> constMessage = new List<AppConstMessage>();

        public AppConst()
        {
            constMessage.Add(new AppConstMessage("Test message", "DF8D1", "ERROR"));
            constMessage.Add(new AppConstMessage(FKviolation, "FK_Exec", "ERROR"));
            //
        }

        public string LoginAndEmailExist
        {
            get { return "Login or Email Exist! "; }
        }

        public string ApprovalSuccess
        {
            get { return "Document Approved! "; }
        }
        public string ApprovalFailure
        {
            get { return "Approval Failed! "; }
        }
        public string SendForApproval
        {
            get { return "Document Sent For Approval! "; }
        }
        public string SendForApprovalFailure
        {
            get { return "Sending For Approval Failed! "; }
        }

        public string RejectSuccess
        {
            get { return "Document Rejected! "; }
        }
        public string RejectFailure
        {
            get { return "Rejection Failed! "; }
        }
        public string InsertFailure
        {
            get { return "Insertion Not Successfull! "; }
        }

        public string InsertSuccess
        {
            get { return "Values Saved Successfully ! "; }
        }

        public string UpdateFailure
        {
            get { return "Updation Not Successfull! "; }
        }

        public string UpdateSuccess
        {
            get { return "Updation Successfull! "; }
        }

        public string NotificationSuccess
        {
            get { return "Notification Send Successfully ! "; }
        }


        public string DeleteFailure
        {
            get { return "Deletion Not Successfull! "; }
        }
        public string DeleteSuccess
        {
            get { return "Deletion Successfull! "; }
        }
        public string FKviolation
        {
            get { return "Deletion Not Successfull!-Already In Use"; }
        }
        public string Duplicate
        {
            get { return "Already Exist.."; }
        }

        public string NoItems
        {
            get { return "No items"; }
        }

        public string PasswordError
        {
            get { return "Password is wrong"; }
        }
        public string MailFailure
        {
            get { return "Mail Sending Failed! "; }
        }

        public string MailSuccess
        {
            get { return "Mail Send Successfully ! "; }
        }
        public AppConstMessage GetMessage(string messageCode)
        {
            AppConstMessage result = new AppConstMessage(messageCode, "", "ERROR");

            try
            {
                foreach (AppConstMessage c in constMessage)
                {
                    if (c.Code == messageCode)
                    {
                        result = c;
                        break;
                    }

                }

            }
            catch (Exception)
            {


            }
            return result;



        }


        #endregion Messages

        #region Strings
        public string AppUser
        {
            get { return "App User"; }
        }
        #endregion
        
    }

    public class AppConstMessage
    {
        public string Message;
        public string Code;
        public string Type;
        public AppConstMessage(string message, string code, string type)
        {
            Message = (code == "" ? "" : code + "-") + message;
            Code = code;
            Type = type;

        }
    }
    public class FileUpload
    {
        public Guid ID { get; set; }
        public Guid ParentID { get; set; }
        public string ParentType { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public string AttachmentURL { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
    }

}