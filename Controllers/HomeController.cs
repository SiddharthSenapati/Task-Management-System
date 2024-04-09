using BusinessAccess;
//using DataAccess;
using First_MVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Microsoft.AspNet.Identity;
using static First_MVC.Models.Employee;

namespace First_MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();

        }
        [HttpGet]
        public ActionResult Forgot_Password()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Change_Password(string uid)
        {
           // string Confirm_Password = Request["newCpwd"];
            if (!string.IsNullOrEmpty(Request["UID"]))
            {
                string Encrypted_id = Request["UID"];
                BAL obj = new BAL();
                string id = obj.Decrypt(Encrypted_id);
                TempData["UID"] = id;
                //PostByString(id);
            }
                return View();
        }

        public ActionResult PostByRequest()
        {
            string email = Request["Email"];
            BAL obj = new BAL();
            int UID = obj.GetUIDByEmail(email);
            obj.Send_Mail(UID, email);
            //YourAction();
           //ShowMessage("Success!...");
            return RedirectToAction("Forgot_Password");
            //Response.Confirm()
            
        }
        
        [HttpPost]
        public ActionResult PostByString()
        {
            //string UID = Request["UID"];
            string UID = TempData["UID"].ToString();
            string password = Request["newPwd"];
            string Confirm_Password = Request["newCpwd"];
                if (password != Confirm_Password)
                {
                    ViewBag.Message = "Password did not match!....";
                }
                else
                {
                    BAL obj1 = new BAL();
                    obj1.ChangePassword(password, UID);
                }
            
            return RedirectToAction("Index");

        }
       
    }
}
