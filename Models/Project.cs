using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace First_MVC.Models
{
    public class Project
    {
        public List<SelectListItem> Projects { get; set; }
        public int ProjectID { get; set; }
        public string ProjectNumber { get; set; }


        public List<SelectListItem> EmployeesList { get; set; }
        public int EmpID { get; set; }
    }
}