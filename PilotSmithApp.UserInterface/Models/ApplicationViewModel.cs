using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PilotSmithApp.UserInterface.Models
{
    public class ApplicationViewModel
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Please Enter Application name")]
        [Display(Name = "Application Name")]
        public string Name { get; set; }
        public PSASysCommonViewModel commonDetails { get; set; }
    }
}