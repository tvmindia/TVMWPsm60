using AutoMapper;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class ExcelExportController : Controller
    {
        IEnquiryBusiness _enquiryBusiness;
        public ExcelExportController(IEnquiryBusiness enquiryBusiness)
        {
            _enquiryBusiness = enquiryBusiness;
        }
        // GET: ExcelExport
        public ActionResult Index()
        {
            return View();
        }
      

        public void DownloadExcel(ExcelExportViewModel excelExportVM)
        {
            try
            {
                string fileName = "";
                PSASysCommon pSASysCommon = new PSASysCommon();
                ExcelPackage excel = new ExcelPackage();

                switch (excelExportVM.DocumentType)
                {
                    case "ENQ":
                        fileName = "Enquiry" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        EnquiryAdvanceSearchViewModel enquiryAdvanceSearchVM = new EnquiryAdvanceSearchViewModel();
                        object ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        enquiryAdvanceSearchVM = JsonConvert.DeserializeObject<EnquiryAdvanceSearchViewModel>(ReadableFormat);
                        List<EnquiryViewModel> enquiryVMList = Mapper.Map<List<Enquiry>, List<EnquiryViewModel>>(_enquiryBusiness.GetAllEnquiry(Mapper.Map<EnquiryAdvanceSearchViewModel, EnquiryAdvanceSearch>(enquiryAdvanceSearchVM)));
                        var workSheet = excel.Workbook.Worksheets.Add("Enquiry");
                        EnquiryViewModel[] enquiryVMListArray = enquiryVMList.ToArray();
                        workSheet.Cells[1, 1].LoadFromCollection(enquiryVMListArray.Select(x => new { EnquiryNo = x.EnquiryNo, ContactPerson = x.Customer.ContactPerson, CompanyName = x.Customer.CompanyName, RequirementSpecification = x.RequirementSpec, Area = x.Area.Description, ReferencePerson = x.ReferencePerson.Name, DocumentOwner = x.PSAUser.LoginName, DocumentStatus = x.DocumentStatus.Description, Branch = x.Branch.Description }), true,TableStyles.Light1);
                        workSheet.Column(1).AutoFit();
                        workSheet.Column(2).AutoFit();
                        workSheet.Column(3).AutoFit();
                        workSheet.Column(4).Width = 40;
                        workSheet.Column(5).AutoFit();
                        workSheet.Column(6).AutoFit();
                        workSheet.Column(7).AutoFit();
                        workSheet.Column(8).AutoFit();
                        workSheet.Column(9).AutoFit();
                        break;
                    case "EST":
                        break;
                    case "QUO":
                        break;
                    case "SOD":
                        break;
                    case "POD":
                        break;
                    case "PQC":
                        break;
                    case "SIV":
                        break;
                    case "SRC":
                        break;
                }
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename="+fileName+".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);                    
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}