using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class ProductionQCViewModel
    {
        public Guid ID { get; set; }
        public string ProdQCNo { get; set; }
        public string ProdQCRefNo { get; set; }
        public DateTime ProdQCDate { get; set; }
        public Guid ProdOrderID { get; set; }
        public Guid CustomerID { get; set; }
        public int PlantCode { get; set; }
        public Guid PreparedBy { get; set; }
        public int DocumentStatusCode { get; set; }
        public string GeneralNotes { get; set; }
        public Guid DocumentOwnerID { get; set; }
        public bool EmailSentYN { get; set; }
        public Guid LatestApprovalID { get; set; }
        public int LatestApprovalStatus { get; set; }
        public bool IsFinalApproved { get; set; }
        public string EmailSentTo { get; set; }
        public int BranchCode { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        //additional fields
        public string DetailJSON { get; set; }
        public string ProdQCDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public List<ProductionQCDetailViewModel> ProductionQCDetailList { get; set; }
        public CustomerViewModel Customer { get; set; }
    }
    public class ProductionQCAdvanceSearchViewModel
    {
        public string QCDate { get; set; }
        public string SearchTerm { get; set; }
        [Display(Name = "Enquiry From")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string FromDate { get; set; }
        [Display(Name = "Enquiry To")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string ToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
    public class ProductionQCDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid ProdQCID { get; set; }
        public Guid ProductID { get; set; }
        public Guid ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal QCQty { get; set; }
        public DateTime QCDate { get; set; }
        public Guid QCBy { get; set; }
        //additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }

    }
}