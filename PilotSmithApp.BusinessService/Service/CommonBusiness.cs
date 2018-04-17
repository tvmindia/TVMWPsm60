using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;

namespace PilotSmithApp.BusinessService.Service
{
    public class CommonBusiness : ICommonBusiness
    {
        ICommonRepository _commonRepository;
        public CommonBusiness(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public string ConvertCurrency(decimal value, int DecimalPoints = 0, bool Symbol = true)
        {
            string result = value.ToString();
            string fare = result;
            decimal parsed = decimal.Parse(fare, CultureInfo.InvariantCulture);
            CultureInfo hindi = new CultureInfo("hi-IN");
            if (Symbol)
                result = string.Format(hindi, "{0:C" + DecimalPoints + "}", parsed);
            else
            {
                if (DecimalPoints == 0)
                { result = string.Format(hindi, "{0:#,#.##}", parsed); }
                else
                { result = string.Format(hindi, "{0:#,#0.00}", parsed); }
            }
            return result;

        }


        private int getMAndatoryIndex(object myObj, string mandatoryProperties)
        {

            int mandIndx = -1;

            object tmp = myObj;
            var ppty = GetProperties(tmp);
            int i;
            for (i = 0; i < ppty.Length; i++)
            {

                if (ppty[i].Name == mandatoryProperties)
                {
                    mandIndx = i;
                    break;
                }

            }

            return mandIndx;


        }

        private void XML(object some_object, int mandIndx, ref string result, ref int totalRows)
        {

            var properties = GetProperties(some_object);
            var mand = properties[mandIndx].GetValue(some_object, null);

            if ((mand != null) && (!string.IsNullOrEmpty(mand.ToString())))
            {

                result = result + "<item ";


                foreach (var p in properties)
                {
                    string name = p.Name;
                    var value = p.GetValue(some_object, null);
                    result = result + " " + name + @"=""" + value + @""" ";

                }
                result = result + "></item>";
                totalRows = totalRows + 1;
            }
        }

        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }
        public string GetXMLfromEnquiryObject(List<EnquiryDetail> enquiryDetailList, string mandatoryProperties)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int mandIndx = getMAndatoryIndex(enquiryDetailList[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in enquiryDetailList)
                {
                    XML(some_object, mandIndx, ref result, ref totalRows);

                }

                result = result + "</Details>";


            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (totalRows > 0)
            {
                return result;
            }
            else
            {
                return "";
            }

        }
        public string GetXMLfromQuotationObject(List<QuotationDetail> quotationDetailList, string mandatoryProperties)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int mandIndx = getMAndatoryIndex(quotationDetailList[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in quotationDetailList)
                {
                    XML(some_object, mandIndx, ref result, ref totalRows);

                }

                result = result + "</Details>";


            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (totalRows > 0)
            {
                return result;
            }
            else
            {
                return "";
            }

        }

        public string GetXMLfromEstimateObject(List<EstimateDetail> estimateDetailList, string mandatoryProperties)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int mandIndx = getMAndatoryIndex(estimateDetailList[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in estimateDetailList)
                {
                    XML(some_object, mandIndx, ref result, ref totalRows);

                }

                result = result + "</Details>";


            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (totalRows > 0)
            {
                return result;
            }
            else
            {
                return "";
            }

        }

        //Send Message
        #region messageSending
        public string SendMessage(string message, string MobileNo, string provider, string type)
        {           
            return _commonRepository.SendMessage(message, MobileNo, provider, type);
        }
        #endregion messageSending
    }
}