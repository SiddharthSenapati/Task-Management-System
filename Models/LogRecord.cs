using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace First_MVC.Models
{
    public class LogRecord                              //Adding Users
    {
        public DateTime Timestamp { get; set; }
        //public string Action { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeID { get; set; }
    }
   
}