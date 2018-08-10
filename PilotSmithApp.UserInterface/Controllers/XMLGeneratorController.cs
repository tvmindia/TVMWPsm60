using AutoMapper;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Xml;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class XMLGeneratorController : Controller
    {
        // GET: XMLGenerator
        ISaleInvoiceBusiness _saleInvoiceBusiness;
        public XMLGeneratorController(ISaleInvoiceBusiness saleInvoiceBusiness)
        {
            _saleInvoiceBusiness = saleInvoiceBusiness;
        }
        public ActionResult Index()
        {
            return View();
        }
        public void GetXmlForTally(string ids, string cookieValue)
        {
            List<SaleInvoiceViewModel> SaleInvoiceVMList;
                SaleInvoiceVMList = Mapper.Map<List<SaleInvoice>, List<SaleInvoiceViewModel>>(_saleInvoiceBusiness.GetSaleInvoiceByID(ids));
            for (var i = 0; i < SaleInvoiceVMList.Count; i++)
            {
                SaleInvoiceVMList[i].SaleInvoiceDetailList = Mapper.Map<List<SaleInvoiceDetail>, List<SaleInvoiceDetailViewModel>>(_saleInvoiceBusiness.GetSaleInvoiceDetailListBySaleInvoiceID(SaleInvoiceVMList[i].ID));
                foreach(SaleInvoiceDetailViewModel saleDetail in SaleInvoiceVMList[i].SaleInvoiceDetailList)
                {
                    SaleInvoiceVMList[i].TotalAmount = (SaleInvoiceVMList[i].TotalAmount == null ? 0: SaleInvoiceVMList[i].TotalAmount) + (saleDetail.Qty * saleDetail.Rate);
                    if (saleDetail.CGSTPerc != 0)
                    {
                        SaleInvoiceVMList[i].CGSTTotal = (SaleInvoiceVMList[i].CGSTTotal == null ? 0 : SaleInvoiceVMList[i].CGSTTotal) + ((saleDetail.Qty * saleDetail.Rate * saleDetail.CGSTPerc) / 100);
                        SaleInvoiceVMList[i].SGSTTotal = (SaleInvoiceVMList[i].SGSTTotal == null ? 0 : SaleInvoiceVMList[i].SGSTTotal) + ((saleDetail.Qty * saleDetail.Rate * saleDetail.SGSTPerc) / 100);
                    }
                    if(saleDetail.IGSTPerc!=0)
                    SaleInvoiceVMList[i].IGSTTotal = (SaleInvoiceVMList[i].IGSTTotal == null ? 0 : SaleInvoiceVMList[i].IGSTTotal) + ((saleDetail.Qty * saleDetail.Rate * saleDetail.IGSTPerc) / 100);
                }
                
            }
            StringBuilder sb = new StringBuilder();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (XmlTextWriter writer = new XmlTextWriter(memoryStream, System.Text.Encoding.UTF8))
                {
                    //XmlTextWriter writer = new XmlTextWriter(res, System.Text.Encoding.UTF8);
                    writer.WriteStartDocument(true);
                    writer.Formatting = Formatting.Indented;
                    writer.Indentation = 2;
                    writer.WriteStartElement("ENVELOPE");
                        writer.WriteStartElement("HEADER");
                        writer.WriteStartElement("TALLYREQUEST");
                        writer.WriteString("Import Data");
                        writer.WriteEndElement();//</TALLYREQUEST>
                        writer.WriteEndElement();//</HEADER>
                        writer.WriteStartElement("BODY");
                        //saleinvoicedetail
                            foreach (SaleInvoiceViewModel saleInvoice in SaleInvoiceVMList)
                            {
                                writer.WriteStartElement("IMPORTDATA");
                                    writer.WriteStartElement("REQUESTDESC");
                                        writer.WriteStartElement("REPORTNAME");
                                            writer.WriteString("Vouchers");
                                        writer.WriteEndElement();//</REPORTNAME>
                                        writer.WriteStartElement("STATICVARIABLES");
                                            writer.WriteStartElement("SVCURRENTCOMPANY");
                                                writer.WriteString(saleInvoice.TallyCompanyName);
                                            writer.WriteEndElement();//</SVCURRENTCOMPANY>
                                        writer.WriteEndElement();//</STATICVARIABLES>
                                    writer.WriteEndElement();//</REQUESTDESC>
                                    writer.WriteStartElement("REQUESTDATA");
                                        writer.WriteStartElement("TALLYMESSAGE");
                                            writer.WriteAttributeString("xmlns:UDF", "TallyUDF");
                                            writer.WriteStartElement("VOUCHER");
                                                writer.WriteAttributeString("VCHTYPE", "Sales");
                                                writer.WriteAttributeString("ACTION", "Create");
                                                writer.WriteAttributeString("OBJVIEW", "Invoice Voucher View");
                                                writer.WriteAttributeString("REMOTEID", saleInvoice.ID.ToString());
                                                writer.WriteStartElement("DATE");
                                                    writer.WriteString(saleInvoice.SaleInvDateTallyFormatted.ToString());
                                                writer.WriteEndElement();//</DATE>
                                                writer.WriteStartElement("STATENAME");
                                                    writer.WriteString("Kerala");
                                                writer.WriteEndElement();//</STATENAME>
                                                writer.WriteStartElement("COUNTRYOFRESIDENCE");
                                                    writer.WriteString("India");
                                                writer.WriteEndElement();//</COUNTRYOFRESIDENCE>
                                                writer.WriteStartElement("PARTYNAME");
                                                    writer.WriteString(saleInvoice.Customer.TallyName);
                                                writer.WriteEndElement();//</PARTYNAME>
                                                writer.WriteStartElement("VOUCHERTYPENAME");
                                                    writer.WriteString("Sales");
                                                writer.WriteEndElement();//</VOUCHERTYPENAME>
                                                writer.WriteStartElement("REFERENCE");
                                                    writer.WriteString(saleInvoice.ReferenceNo);
                                                writer.WriteEndElement();//</REFERENCE>
                                                writer.WriteStartElement("VOUCHERNUMBER");
                                                    writer.WriteString(saleInvoice.SaleInvNo);
                                                writer.WriteEndElement();//</VOUCHERNUMBER>
                                                writer.WriteStartElement("PARTYLEDGERNAME");
                                                    writer.WriteString(saleInvoice.Customer.TallyName);
                                                writer.WriteEndElement();//</PARTYLEDGERNAME>
                                                writer.WriteStartElement("BASICBASEPARTYNAME");
                                                    writer.WriteString(saleInvoice.Customer.TallyName);
                                                writer.WriteEndElement();//</BASICBASEPARTYNAME>
                                                writer.WriteStartElement("FBTPAYMENTTYPE");
                                                    writer.WriteString("Default");
                                                writer.WriteEndElement();//</FBTPAYMENTTYPE>
                                                writer.WriteStartElement("PERSISTEDVIEW");
                                                    writer.WriteString("Invoice Voucher View");
                                                writer.WriteEndElement();//</PERSISTEDVIEW>
                                                writer.WriteStartElement("PLACEOFSUPPLY");
                                                    writer.WriteString("Kerala");
                                                writer.WriteEndElement();//</PLACEOFSUPPLY>
                                                writer.WriteStartElement("BASICBUYERNAME");
                                                    writer.WriteString(saleInvoice.Customer.TallyName);
                                                writer.WriteEndElement();//</BASICBUYERNAME>
                                                writer.WriteStartElement("VCHGSTCLASS");
                                                writer.WriteEndElement();//</VCHGSTCLASS>
                                                writer.WriteStartElement("CONSIGNEESTATENAME");
                                                    writer.WriteString("Kerala");
                                                writer.WriteEndElement();//</CONSIGNEESTATENAME>
                                                writer.WriteStartElement("ISINVOICE");
                                                    writer.WriteString("Yes");
                                                writer.WriteEndElement();//</ISINVOICE>
                                                writer.WriteStartElement("MFGJOURNAL");
                                                    writer.WriteString("No");
                                                writer.WriteEndElement();//</MFGJOURNAL>
                                                writer.WriteStartElement("HASDISCOUNTS");
                                                    writer.WriteString("No");
                                                writer.WriteEndElement();//</HASDISCOUNTS>
                                                writer.WriteStartElement("LEDGERENTRIES.LIST");
                                                    writer.WriteStartElement("OLDAUDITENTRYIDS.LIST");
                                                        writer.WriteAttributeString("TYPE", "Number");
                                                        writer.WriteStartElement("OLDAUDITENTRYIDS");
                                                            writer.WriteString("-1");
                                                        writer.WriteEndElement();//</OLDAUDITENTRYIDS>
                                                    writer.WriteFullEndElement();//</OLDAUDITENTRYIDS.LIST>
                                                    writer.WriteStartElement("LEDGERNAME");
                                                        writer.WriteString(saleInvoice.Customer.TallyName);
                                                    writer.WriteEndElement();//</LEDGERNAME>
                                                    writer.WriteStartElement("GSTCLASS");
                                                    writer.WriteEndElement();//</GSTCLASS>
                                                    writer.WriteStartElement("ISDEEMEDPOSITIVE");
                                                        writer.WriteString("No");
                                                    writer.WriteEndElement();//</ISDEEMEDPOSITIVE>
                                                    writer.WriteStartElement("LEDGERFROMITEM");
                                                        writer.WriteString("No");
                                                    writer.WriteEndElement();//</LEDGERFROMITEM>
                                                    writer.WriteStartElement("REMOVEZEROENTRIES");
                                                        writer.WriteString("No");
                                                    writer.WriteEndElement();//</REMOVEZEROENTRIES>
                                                    writer.WriteStartElement("ISPARTYLEDGER");
                                                        writer.WriteString("Yes");
                                                    writer.WriteEndElement();//</ISPARTYLEDGER>
                                                    writer.WriteStartElement("ISLASTDEEMEDPOSITIVE");
                                                        writer.WriteString("Yes");
                                                    writer.WriteEndElement();//</ISLASTDEEMEDPOSITIVE>
                                                    writer.WriteStartElement("ISCAPVATTAXALTERED");
                                                        writer.WriteString("No");
                                                    writer.WriteEndElement();//</ISCAPVATTAXALTERED>
                                                    writer.WriteStartElement("AMOUNT");
                                                        writer.WriteString((-1 * (saleInvoice.TotalAmount+saleInvoice.CGSTTotal+saleInvoice.SGSTTotal)).ToString());
                                                    writer.WriteEndElement();//</AMOUNT>
                                                writer.WriteEndElement();//</LEDGERENTRIES.LIST>

                                                if(saleInvoice.CGSTTotal!=0 && saleInvoice.CGSTTotal != null)
                                                {
                                                    writer.WriteStartElement("LEDGERENTRIES.LIST");
                                                        writer.WriteStartElement("OLDAUDITENTRYIDS.LIST");
                                                            writer.WriteAttributeString("TYPE", "Number");
                                                            writer.WriteStartElement("OLDAUDITENTRYIDS");
                                                            writer.WriteString("-1");
                                                        writer.WriteEndElement();//</OLDAUDITENTRYIDS>
                                                        writer.WriteFullEndElement();//</OLDAUDITENTRYIDS.LIST>
                                                        writer.WriteStartElement("LEDGERNAME");
                                                            writer.WriteString(saleInvoice.SGSTTallyLedger);
                                                        writer.WriteEndElement();//</LEDGERNAME>
                                                        writer.WriteStartElement("GSTCLASS");
                                                        writer.WriteEndElement();//</GSTCLASS>
                                                        writer.WriteStartElement("ISDEEMEDPOSITIVE");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISDEEMEDPOSITIVE>
                                                        writer.WriteStartElement("LEDGERFROMITEM");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</LEDGERFROMITEM>
                                                        writer.WriteStartElement("REMOVEZEROENTRIES");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</REMOVEZEROENTRIES>
                                                        writer.WriteStartElement("ISPARTYLEDGER");
                                                            writer.WriteString("Yes");
                                                        writer.WriteEndElement();//</ISPARTYLEDGER>
                                                        writer.WriteStartElement("ISLASTDEEMEDPOSITIVE");
                                                            writer.WriteString("Yes");
                                                        writer.WriteEndElement();//</ISLASTDEEMEDPOSITIVE>
                                                        writer.WriteStartElement("ISCAPVATTAXALTERED");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISCAPVATTAXALTERED>
                                                        writer.WriteStartElement("ISCAPVATNOTCLAIMED");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISCAPVATNOTCLAIMED>
                                                        writer.WriteStartElement("AMOUNT");
                                                            writer.WriteString((saleInvoice.CGSTTotal).ToString());
                                                        writer.WriteEndElement();//</AMOUNT>
                                                    writer.WriteEndElement();//</LEDGERENTRIES.LIST>

                                                    writer.WriteStartElement("LEDGERENTRIES.LIST");
                                                        writer.WriteStartElement("OLDAUDITENTRYIDS.LIST");
                                                        writer.WriteAttributeString("TYPE", "Number");
                                                            writer.WriteStartElement("OLDAUDITENTRYIDS");
                                                            writer.WriteString("-1");
                                                            writer.WriteEndElement();//</OLDAUDITENTRYIDS>
                                                        writer.WriteFullEndElement();//</OLDAUDITENTRYIDS.LIST>
                                                        writer.WriteStartElement("LEDGERNAME");
                                                            writer.WriteString(saleInvoice.CGSTTallyLedger);
                                                        writer.WriteEndElement();//</LEDGERNAME>
                                                        writer.WriteStartElement("GSTCLASS");
                                                        writer.WriteEndElement();//</GSTCLASS>
                                                        writer.WriteStartElement("ISDEEMEDPOSITIVE");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISDEEMEDPOSITIVE>
                                                        writer.WriteStartElement("LEDGERFROMITEM");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</LEDGERFROMITEM>
                                                        writer.WriteStartElement("REMOVEZEROENTRIES");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</REMOVEZEROENTRIES>
                                                        writer.WriteStartElement("ISPARTYLEDGER");
                                                            writer.WriteString("Yes");
                                                        writer.WriteEndElement();//</ISPARTYLEDGER>
                                                        writer.WriteStartElement("ISLASTDEEMEDPOSITIVE");
                                                            writer.WriteString("Yes");
                                                        writer.WriteEndElement();//</ISLASTDEEMEDPOSITIVE>
                                                        writer.WriteStartElement("ISCAPVATTAXALTERED");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISCAPVATTAXALTERED>
                                                        writer.WriteStartElement("ISCAPVATNOTCLAIMED");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISCAPVATNOTCLAIMED>
                                                        writer.WriteStartElement("AMOUNT");
                                                            writer.WriteString((saleInvoice.SGSTTotal).ToString());
                                                        writer.WriteEndElement();//</AMOUNT>
                                                    writer.WriteEndElement();//</LEDGERENTRIES.LIST>
                                                }

                                                if(saleInvoice.IGSTTotal!=0 && saleInvoice.IGSTTotal != null)
                                                 {
                                                    writer.WriteStartElement("LEDGERENTRIES.LIST");
                                                        writer.WriteStartElement("OLDAUDITENTRYIDS.LIST");
                                                        writer.WriteAttributeString("TYPE", "Number");
                                                            writer.WriteStartElement("OLDAUDITENTRYIDS");
                                                            writer.WriteString("-1");
                                                            writer.WriteEndElement();//</OLDAUDITENTRYIDS>
                                                        writer.WriteFullEndElement();//</OLDAUDITENTRYIDS.LIST>
                                                        writer.WriteStartElement("LEDGERNAME");
                                                            writer.WriteString(saleInvoice.IGSTTallyLedger);
                                                        writer.WriteEndElement();//</LEDGERNAME>
                                                        writer.WriteStartElement("GSTCLASS");
                                                        writer.WriteEndElement();//</GSTCLASS>
                                                        writer.WriteStartElement("ISDEEMEDPOSITIVE");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISDEEMEDPOSITIVE>
                                                        writer.WriteStartElement("LEDGERFROMITEM");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</LEDGERFROMITEM>
                                                        writer.WriteStartElement("REMOVEZEROENTRIES");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</REMOVEZEROENTRIES>
                                                        writer.WriteStartElement("ISPARTYLEDGER");
                                                            writer.WriteString("Yes");
                                                        writer.WriteEndElement();//</ISPARTYLEDGER>
                                                        writer.WriteStartElement("ISLASTDEEMEDPOSITIVE");
                                                            writer.WriteString("Yes");
                                                        writer.WriteEndElement();//</ISLASTDEEMEDPOSITIVE>
                                                        writer.WriteStartElement("ISCAPVATTAXALTERED");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISCAPVATTAXALTERED>
                                                        writer.WriteStartElement("ISCAPVATNOTCLAIMED");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISCAPVATNOTCLAIMED>
                                                        writer.WriteStartElement("AMOUNT");
                                                            writer.WriteString((saleInvoice.IGSTTotal).ToString());
                                                        writer.WriteEndElement();//</AMOUNT>
                                                    writer.WriteEndElement();//</LEDGERENTRIES.LIST>
                                                  }

                                                foreach (SaleInvoiceDetailViewModel saleInvoiceDetail in saleInvoice.SaleInvoiceDetailList)
                                                {
                                                    writer.WriteStartElement("ALLINVENTORYENTRIES.LIST");
                                                        writer.WriteStartElement("STOCKITEMNAME");
                                                            writer.WriteString(saleInvoiceDetail.Product.TallyName.ToString());
                                                        writer.WriteEndElement();//</STOCKITEMNAME>
                                                        writer.WriteStartElement("ISDEEMEDPOSITIVE");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISDEEMEDPOSITIVE>
                                                        writer.WriteStartElement("ISLASTDEEMEDPOSITIVE");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISLASTDEEMEDPOSITIVE>
                                                        writer.WriteStartElement("ISAUTONEGATE");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISAUTONEGATE>
                                                        writer.WriteStartElement("ISCUSTOMSCLEARANCE");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISCUSTOMSCLEARANCE>
                                                        writer.WriteStartElement("ISTRACKCOMPONENT");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISTRACKCOMPONENT>
                                                        writer.WriteStartElement("ISTRACKPRODUCTION");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISTRACKPRODUCTION>
                                                        writer.WriteStartElement("ISPRIMARYITEM");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISPRIMARYITEM>
                                                        writer.WriteStartElement("ISSCRAP");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</ISSCRAP>
                                                        writer.WriteStartElement("RATE");
                                                            writer.WriteString(saleInvoiceDetail.Rate.ToString()+ "/");
                                                            writer.WriteString(saleInvoiceDetail.Unit.Description.ToString());
                                                        writer.WriteEndElement();//</RATE>
                                                        writer.WriteStartElement("AMOUNT");
                                                            writer.WriteString((saleInvoiceDetail.Qty * saleInvoiceDetail.Rate).ToString());
                                                        writer.WriteEndElement();//</AMOUNT>
                                                        writer.WriteStartElement("ACTUALQTY");
                                                            writer.WriteString(saleInvoiceDetail.Qty.ToString());
                                                            writer.WriteString(saleInvoiceDetail.Unit.Description.ToString());
                                                        writer.WriteEndElement();//</ACTUALQTY>
                                                        writer.WriteStartElement("BILLEDQTY");
                                                            writer.WriteString(saleInvoiceDetail.Qty.ToString());
                                                            writer.WriteString(saleInvoiceDetail.Unit.Description.ToString());
                                                        writer.WriteEndElement();//</BILLEDQTY>
                                                        writer.WriteStartElement("BATCHALLOCATIONS.LIST");
                                                        writer.WriteStartElement("GODOWNNAME");
                                                            writer.WriteString("Main Location");
                                                        writer.WriteEndElement();//</GODOWNNAME>
                                                        writer.WriteStartElement("BATCHNAME");
                                                            writer.WriteString("Primary Batch");
                                                        writer.WriteEndElement();//</BATCHNAME>
                                                        writer.WriteStartElement("INDENTNO");
                                                        writer.WriteEndElement();//</INDENTNO>
                                                        writer.WriteStartElement("ORDERNO");
                                                        writer.WriteEndElement();//</ORDERNO>
                                                        writer.WriteStartElement("TRACKINGNUMBER");
                                                        writer.WriteEndElement();//</TRACKINGNUMBER>
                                                        writer.WriteStartElement("DYNAMICCSTISCLEARED");
                                                            writer.WriteString("No");
                                                        writer.WriteEndElement();//</DYNAMICCSTISCLEARED>
                                                        writer.WriteStartElement("AMOUNT");
                                                            writer.WriteString((saleInvoiceDetail.Qty * saleInvoiceDetail.Rate).ToString());
                                                        writer.WriteEndElement();//</AMOUNT>
                                                        writer.WriteStartElement("ACTUALQTY");
                                                            writer.WriteString(saleInvoiceDetail.Qty.ToString());
                                                            writer.WriteString(saleInvoiceDetail.Unit.Description.ToString());
                                                        writer.WriteEndElement();//</ACTUALQTY>
                                                        writer.WriteStartElement("BILLEDQTY");
                                                            writer.WriteString(saleInvoiceDetail.Qty.ToString());
                                                            writer.WriteString(saleInvoiceDetail.Unit.Description.ToString());
                                                        writer.WriteEndElement();//</BILLEDQTY>
                                                        writer.WriteEndElement();//</BATCHALLOCATIONS.LIST>
                                                        writer.WriteStartElement("ACCOUNTINGALLOCATIONS.LIST");
                                                            writer.WriteStartElement("LEDGERNAME");
                                                                if(saleInvoiceDetail.TaxType.TallyName!=null)
                                                                writer.WriteString(saleInvoiceDetail.TaxType.TallyName);
                                                                else
                                                                writer.WriteString("Direct Sale");
                                                                writer.WriteEndElement();//</LEDGERNAME>
                                                            writer.WriteStartElement("AMOUNT");
                                                                writer.WriteString((saleInvoiceDetail.Qty * saleInvoiceDetail.Rate).ToString());
                                                            writer.WriteEndElement();//</AMOUNT>
                                                        writer.WriteEndElement();//</ACCOUNTINGALLOCATIONS.LIST>
                                                    writer.WriteEndElement();//</ALLINVENTORYENTRIES.LIST>
                                                }
                                            writer.WriteFullEndElement();//</VOUCHER>
                                        writer.WriteFullEndElement();//</TALLYMESSAGE>
                                    writer.WriteEndElement();//</REQUESTDATA>
                                writer.WriteEndElement();//</IMPORTDATA>

                                //}
                            }
                        writer.WriteEndElement();//</BODY>
                    writer.WriteEndElement();//</ENVELOPE>
                    writer.WriteEndDocument();
                }
                sb.Append(Encoding.UTF8.GetString(memoryStream.ToArray()));
                string text = sb.ToString();
                Response.Buffer = true;
                //Response.BufferOutput = false;
                Response.ContentType = "application/xml";
                Response.AddHeader("content-disposition", "attachment;  filename=saleinvoiceXMLFortally" + ".xml");
                //memoryStream.WriteTo(Response.OutputStream);
                // add a cookie with the name 'dlc' and the value from the postback
                ControllerContext.HttpContext.Response.Cookies.Add(new HttpCookie("dlc", cookieValue));
                Response.Write(text);
                Response.Flush();
                Response.End();
                //writer.Flush();
                //writer.Close();
                memoryStream.Close();
            }

            //}
            

        }
    }
}