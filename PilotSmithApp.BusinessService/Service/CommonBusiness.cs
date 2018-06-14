using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System.Linq;

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


        private int[] getMAndatoryIndex(object myObj, string mandatoryProperties)
        {

            int[] mandIndx = new int[mandatoryProperties.Split(',').Count()];
            string[] mandatoryList = mandatoryProperties.Split(',');
            object tmp = myObj;
            var ppty = GetProperties(tmp);
            int i;
            int j = 0;
            for (i = 0; i < ppty.Length; i++)
            {
                if (mandatoryList.Where(x => x == ppty[i].Name).Count() > 0)
                {
                    mandIndx[j] = i;
                    j = j + 1;
                }

            }

            return mandIndx;


        }

        private void XML(object some_object, int[] mandIndx, ref string result, ref int totalRows)
        {

            var properties = GetProperties(some_object);
            object[] mand = new object[mandIndx.Count()];
            int j = 0;
            foreach (int i in mandIndx)
            {
                mand[j] = properties[i].GetValue(some_object, null) == (object)Guid.Empty ? null : properties[i].GetValue(some_object, null);
                j = j + 1;
            }
            if (mand.Where(x => x == null || x == (object)Guid.Empty).Count() == 0)
            {

                result = result + "<item ";


                foreach (var p in properties)
                {
                    string name = p.Name;
                    var value = p.GetValue(some_object, null);
                    //result = result + " " + name + @"=""" + value + @""" ";
                    result = result = result + " " + name + @"=""" + (value != null ? value.ToString().Replace("\"", "&quot;").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;") : value) + @""" ";


                }
                result = result + "></item>";
                totalRows = totalRows + 1;
            }
            else
            {
                throw new Exception("Mandatory fields in Detail is missing");
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
                int[] mandIndx = getMAndatoryIndex(enquiryDetailList[0], mandatoryProperties); //int mandIndx = 0;                

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
                int[] mandIndx = getMAndatoryIndex(quotationDetailList[0], mandatoryProperties); //int mandIndx = 0;                

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

        public string GetXMLfromQuotationOtherChargeObject(List<QuotationOtherCharge> quotationOtherChargeList, string mandatoryProperties)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int[] mandIndx = getMAndatoryIndex(quotationOtherChargeList[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in quotationOtherChargeList)
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
                int[] mandIndx = getMAndatoryIndex(estimateDetailList[0], mandatoryProperties); //int mandIndx = 0;                

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
        public string GetXMLfromProductionOrderObject(List<ProductionOrderDetail> productionOrderDetailList, string mandatoryProperties)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int[] mandIndx = getMAndatoryIndex(productionOrderDetailList[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in productionOrderDetailList)
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
        public string GetXMLfromProductionQCObject(List<ProductionQCDetail> productionQCDetailList, string mandatoryProperties)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int[] mandIndx = getMAndatoryIndex(productionQCDetailList[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in productionQCDetailList)
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
        public string GetXMLfromSaleInvoiceObject(List<SaleInvoiceDetail> saleInvoiceDetailList, string mandatoryProperties)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int[] mandIndx = getMAndatoryIndex(saleInvoiceDetailList[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in saleInvoiceDetailList)
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
        public string GetXMLfromSaleOrderObject(List<SaleOrderDetail> saleOrderDetailList, string mandatoryProperties)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int[] mandIndx = getMAndatoryIndex(saleOrderDetailList[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in saleOrderDetailList)
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

        public string GetXMLfromSaleOrderOtherChargeObject(List<SaleOrderOtherCharge> saleOrderOtherChargeDetailList, string mandatoryProperties)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int[] mandIndx = getMAndatoryIndex(saleOrderOtherChargeDetailList[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in saleOrderOtherChargeDetailList)
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

        public string GetXMLfromDeliveryChallanObject(List<DeliveryChallanDetail> deliveryChallanDetailList, string mandatoryProperties)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int[] mandIndx = getMAndatoryIndex(deliveryChallanDetailList[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in deliveryChallanDetailList)
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

        public string GetXMLfromServiceCallObject(List<ServiceCallDetail> serviceCallDetailList, string mandatoryProperties)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int[] mandIndx = getMAndatoryIndex(serviceCallDetailList[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in serviceCallDetailList)
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

        public string GetXMLfromServiceCallChargeObject(List<ServiceCallCharge> serviceCallChargeList, string mandatoryProperties)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int[] mandIndx = getMAndatoryIndex(serviceCallChargeList[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in serviceCallChargeList)
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
        public bool CheckDocumentIsDeletable(string docType, Guid? id)
        {
            return _commonRepository.CheckDocumentIsDeletable(docType, id);
        }

        public string GetXMLfromSaleInvoiceOtherChargeObject(List<SaleInvoiceOtherCharge> saleInvoiceOtherChargeDetailList, string mandatoryProperties)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int[] mandIndx = getMAndatoryIndex(saleInvoiceOtherChargeDetailList[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in saleInvoiceOtherChargeDetailList)
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



        public List<TimeLine> GetTimeLine(Guid Id, String Type) {

            _commonRepository.GetTimeLine(Id, Type);
        }
    }
}