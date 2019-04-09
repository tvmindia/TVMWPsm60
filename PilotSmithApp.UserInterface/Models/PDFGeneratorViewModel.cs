using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class PDFGeneratorViewModel
    {
        public ElementStructure table;
        public ElementStructure trodd;
        public ElementStructure treven;
        public ElementStructure th;
        public ElementStructure td;
    }
    public struct ElementStructure
    {
        public string style { get; set; }
        public string text { get; set; }
    }
    public class PDFToolsViewModel
    {
        [AllowHtml]
        public string Content { get; set; }
        [AllowHtml]
        public string Headcontent { get; set; }
        public string HeaderText { get; set; }
        [AllowHtml]
        public string ContentFileName { get; set; }
        [AllowHtml]
        public string CustomerName { get; set; }        
        [Display(Name ="Print On")]
        public bool IsWithLetterHead { get; set; } = true;
        public bool IsWithWaterMark { get; set; } = false;
    }
}