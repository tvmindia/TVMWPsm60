using System.Web.Optimization;

namespace PilotSmithApp.UserInterface.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/Content/boot").Include("~/Content/bootstrap.css", "~/Content/bootstrap-theme.css", "~/Content/font-awesome.min.css", "~/Content/fontawesome.min.css", "~/Content/Custom.css", "~/Content/sweetalert.css"));
            bundles.Add(new StyleBundle("~/Content/AdminLTE/css/plugins").Include("~/Content/AdminLTE/css/jvectormap/jquery-jvectormap-1.2.2.css", "~/Content/AdminLTE/css/AdminLTE.min.css", "~/Content/AdminLTE/css/skins/_all-skins.min.css"));
            //bundles.Add(new StyleBundle("~/AdminLTE/bootstrap/css/plugins").Include("~/AdminLTE/plugins/jvectormap/jquery-jvectormap-1.2.2.css", "~/AdminLTE/dist/css/AdminLTE.min.css", "~/AdminLTE/dist/css/skins/_all-skins.min.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrapdatepicker").Include("~/Content/bootstrap-datepicker3.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatable").Include("~/Content/DataTables/css/dataTables.bootstrap.min.css", "~/Content/DataTables/css/responsive.bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatablecheckbox").Include("~/Content/DataTables/css/dataTables.checkboxes.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableSelect").Include("~/Content/DataTables/css/select.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableButtons").Include("~/Content/DataTables/css/buttons.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableFixedColumns").Include("~/Content/DataTables/css/fixedColumns.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableFixedHeader").Include("~/Content/DataTables/css/fixedHeader.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/MvcDatalist/Datalist").Include("~/Content/MvcDatalist/mvc-datalist.css"));
            bundles.Add(new StyleBundle("~/Content/css/select2").Include("~/Content/css/select2.min.css"));

            //-------------------
            bundles.Add(new StyleBundle("~/Content/UserCSS/Login").Include("~/Content/UserCSS/Login.css"));
            bundles.Add(new StyleBundle("~/Content/css/Select2").Include("~/Content/css/select2.css"));
            bundles.Add(new StyleBundle("~/Content/css/Selectmin").Include("~/Content/css/select2.min.css"));
            bundles.Add(new StyleBundle("~/Content/PSAForms").Include("~/Content/PSAForms.css"));
            //---------------------
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-3.1.1.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include("~/Scripts/jquery.form.js"));
            bundles.Add(new ScriptBundle("~/bundles/AdminLTE").Include("~/Scripts/AdminLTE/fastclick.min.js", "~/Scripts/AdminLTE/adminlte.min.js", "~/Scripts/AdminLTE/jquery.sparkline.min.js", "~/Scripts/AdminLTE/jquery.slimscroll.min.js", "~/Scripts/AdminLTE/Chart.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/AdminLTEDash").Include("~/Scripts/AdminLTE/dashboard2.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryunobtrusiveajaxvalidate").Include("~/Scripts/jquery.validate.min.js", "~/Scripts/jquery.validate.unobtrusive.min.js", "~/Scripts/jquery.unobtrusive-ajax.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include("~/Scripts/DataTables/jquery.dataTables.min.js", "~/Scripts/DataTables/dataTables.bootstrap.min.js", "~/Scripts/DataTables/dataTables.responsive.min.js", "~/Scripts/DataTables/responsive.bootstrap.min.js", "~/Scripts/DataTables/dataTables.fixedHeader.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableSelect").Include("~/Scripts/DataTables/dataTables.select.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatablecheckbox").Include("~/Scripts/DataTables/dataTables.checkboxes.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableButtons").Include("~/Scripts/DataTables/dataTables.buttons.min.js", "~/Scripts/DataTables/buttons.flash.min.js", "~/Scripts/DataTables/buttons.html5.min.js", "~/Scripts/DataTables/buttons.print.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableFixedColumns").Include("~/Scripts/DataTables/dataTables.fixedColumns.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jsZip").Include("~/Scripts/jszip.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/userpluginjs").Include("~/Scripts/jquery.noty.packaged.min.js", "~/Scripts/custom.js", "~/Scripts/UserJS/Master.js", "~/Scripts/Chart.js", "~/Scripts/sweetalert.min.js", "~/Scripts/bootstrap-notify.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrapdatepicker").Include("~/Scripts/bootstrap-datepicker.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/MvcDatalist/DataList").Include("~/Scripts/MvcDatalist/mvc-datalist.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/selectmin").Include("~/Scripts/select2.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/select2").Include("~/Scripts/select2.js"));

            //----------------------
            //----------------------
            bundles.Add(new ScriptBundle("~/bundles/ManageAccess").Include("~/Scripts/UserJS/ManageAccess.js"));
            bundles.Add(new ScriptBundle("~/bundles/ManageSubObjectAccess").Include("~/Scripts/UserJS/ManageSubObjectAccess.js"));
            bundles.Add(new ScriptBundle("~/bundles/Login").Include("~/Scripts/UserJS/Login.js"));
            bundles.Add(new ScriptBundle("~/bundles/User").Include("~/Scripts/UserJS/User.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserBranch").Include("~/Scripts/UserJS/UserBranch.js"));
            bundles.Add(new ScriptBundle("~/bundles/Privileges").Include("~/Scripts/UserJS/Privileges.js"));
            bundles.Add(new ScriptBundle("~/bundles/PrivilegesView").Include("~/Scripts/UserJS/PrivilegesView.js"));
            bundles.Add(new ScriptBundle("~/bundles/Application").Include("~/Scripts/UserJS/Application.js"));
            bundles.Add(new ScriptBundle("~/bundles/AppObject").Include("~/Scripts/UserJS/AppObject.js"));
            bundles.Add(new ScriptBundle("~/bundles/AppSubobject").Include("~/Scripts/UserJS/AppSubobject.js"));
            bundles.Add(new ScriptBundle("~/bundles/Roles").Include("~/Scripts/UserJS/Roles.js"));
            //------------------------------------
            //---------------------------------------
            bundles.Add(new ScriptBundle("~/bundles/UserJS/AdvanceSelectList").Include("~/Scripts/UserJS/AdvanceSelectList.js"));
            bundles.Add(new ScriptBundle("~/bundles/CustomerJS/Customer").Include("~/Scripts/UserJS/CustomerJS/Customer.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/ProductCategory").Include("~/Scripts/UserJS/ProductCategory.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/ProductSpecification").Include("~/Scripts/UserJS/ProductSpecification.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/State").Include("~/Scripts/UserJS/State.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/District").Include("~/Scripts/UserJS/District.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/Area").Include("~/Scripts/UserJS/Area.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/CustomerJS/Customer").Include("~/Scripts/UserJS/CustomerJS/Customer.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/Enquiry").Include("~/Scripts/UserJS/Enquiry.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/Quotation").Include("~/Scripts/UserJS/Quotation.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/Product").Include("~/Scripts/UserJS/Product.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/Company").Include("~/Scripts/UserJS/Company.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/ProductModel").Include("~/Scripts/UserJS/ProductModel.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/Estimate").Include("~/Scripts/UserJS/Estimate.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/DashBoard").Include("~/Scripts/UserJS/SalesSummary.js", "~/Scripts/UserJS/EnquiryFollowupSummary.js", "~/Scripts/UserJS/EnquiryCountSummary.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/DocumentApproval/ApprovalHistory").Include("~/Scripts/UserJS/DocumentApproval/ApprovalHistory.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/DocumentApproval/ApproveDocument").Include("~/Scripts/UserJS/DocumentApproval/ApproveDocument.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/DocumentApproval/DocumentSummary").Include("~/Scripts/UserJS/DocumentApproval/DocumentSummary.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/DocumentApproval/ViewPendingDocuments").Include("~/Scripts/UserJS/DocumentApproval/ViewPendingDocuments.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Approver").Include("~/Scripts/UserJS/Approver.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/CustomerCategory").Include("~/Scripts/UserJS/CustomerCategory.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/SaleOrder").Include("~/Scripts/UserJS/SaleOrder.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/ProductionOrder").Include("~/Scripts/UserJS/ProductionOrder.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/ProductionQC").Include("~/Scripts/UserJS/ProductionQC.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/ProductionQC").Include("~/Scripts/UserJS/ProductionQC.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/Employee").Include("~/Scripts/UserJS/Employee.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/ServiceCall").Include("~/Scripts/UserJS/ServiceCall.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/ReferencePerson").Include("~/Scripts/UserJS/ReferencePerson.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/PaymentTerm").Include("~/Scripts/UserJS/PaymentTerm.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/TaxType").Include("~/Scripts/UserJS/TaxType.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/CustomerCategory").Include("~/Scripts/UserJS/CustomerCategory.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/Plant").Include("~/Scripts/UserJS/Plant.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/OtherCharge").Include("~/Scripts/UserJS/OtherCharge.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/Bank").Include("~/Scripts/UserJS/Bank.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/Spare").Include("~/Scripts/UserJS/Spare.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/SysSetting").Include("~/Scripts/UserJS/SysSetting.js"));


            bundles.Add(new ScriptBundle("~/bundles/UserJS/SaleInvoice").Include("~/Scripts/UserJS/SaleInvoice.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/DeliveryChallan").Include("~/Scripts/UserJS/DeliveryChallan.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/Country").Include("~/Scripts/UserJS/Country.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/DocumentApproval/ViewApprovalHistory").Include("~/Scripts/UserJS/DocumentApproval/ViewApprovalHistory.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/Report").Include("~/Scripts/UserJS/Report/Report.js"));
            //bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/PendingSaleOrderProductionReport").Include("~/Scripts/UserJS/Report/PendingSaleOrderProductionReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/EnquiryReport").Include("~/Scripts/UserJS/Report/EnquiryReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/ReportAdvanceSelectList").Include("~/Scripts/UserJS/Report/ReportAdvanceSelectList.js"));            
            bundles.Add(new ScriptBundle("~/bundles/UserJs/RecentDocument").Include("~/Scripts/UserJS/RecentDocument.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/ProformaInvoice").Include("~/Scripts/UserJS/ProformaInvoice.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/EnquiryFollowupReport").Include("~/Scripts/UserJS/Report/EnquiryFollowupReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/EstimateReport").Include("~/Scripts/UserJS/Report/EstimateReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/QuotationReport").Include("~/Scripts/UserJS/Report/QuotationReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/PendingSaleOrderReport").Include("~/Scripts/UserJS/Report/PendingSaleOrderReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/SaleOrderReport").Include("~/Scripts/UserJS/Report/SaleOrderReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/ProductionOrderStandardReport").Include("~/Scripts/UserJS/Report/ProductionOrderStandardReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/PendingProductionOrderReport").Include("~/Scripts/UserJS/Report/PendingProductionOrderReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/ProductionQCStandardReport").Include("~/Scripts/UserJS/Report/ProductionQCStandardReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/PendingProductionQCReport").Include("~/Scripts/UserJS/Report/PendingProductionQCReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/QuotationDetailReport").Include("~/Scripts/UserJS/Report/QuotationDetailReport.js"));
        }
    }
}