using First_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using DataAccess;
using BusinessAccess;
using static First_MVC.Models.Employee;

namespace First_MVC.Controllers
{
    public class LogInController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Change_Password()
        {
            return View();

        }
        [HttpGet]
        public ActionResult Admin()
        {
            string email = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                email = reqCookies["Email"];        //Accessing The Cookies
            }
            BAL obj = new BAL();
            string name = obj.GetUserNameByEmail(email);
            ViewBag.msg = name;
            return View();
        }
        [HttpGet]
        public ActionResult User()
        {
            string email = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                email = reqCookies["Email"];        //Accessing The Cookies
            }
            BAL obj = new BAL();
            string name = obj.GetUserNameByEmail(email);
            ViewBag.msg = name;
            return View();

        }
        [HttpPost]
        public ActionResult PostByBinding(Employee emp)
        {
            try
            {
                string email = Request["Email"];
                string password = Request["Password"];
                HttpCookie userInfo = new HttpCookie("userInfo");          //sending Cookies
                userInfo["Email"] = email;
                userInfo.Expires.Add(new TimeSpan(0, 1, 0));
                Response.Cookies.Add(userInfo);

                BAL obj = new BAL();
                string link = obj.match(email, password);
                //string originalString = "Hello World";
                string[] parts = link.Split(' ');
                string UserType = parts[0];
                string UserName = parts[1];

                switch (UserType)
                {
                    case "admin":
                        HttpCookie nameCookie = new HttpCookie("nameCookie");          //sending Cookies
                        nameCookie["UserName"] = UserName;
                        nameCookie.Expires.AddDays(30);
                        Response.Cookies.Add(nameCookie);

                        return RedirectToAction("Admin");

                    case "user":
                        return RedirectToAction("User");
                    default:
                        return ViewBag.Message = "Not correct";
                }
               
            }catch(Exception ex)
            {
                throw ex;
            }

            //return Content("");
        }
        [HttpPost]
        public void PostUsingFormCollection(FormCollection form)
        {
            if (!string.IsNullOrEmpty(Request["UserEmail"]))
            {
                string email = Request["UserEmail"];
                BAL obj = new BAL();
                string user = obj.GetUserNameByEmail(email);
                ViewBag.Message = user;
            }
        }

        public void PostByString()
        {
            string password = Request["newPwd"];
            string Confirm_Password = Request["newCpwd"];
            if (!string.IsNullOrEmpty(Request["UID"]))
            {
                string Encrypted_id = Request["UID"];
                BAL obj = new BAL();
                string id = obj.Decrypt(Encrypted_id);

                if (password != Confirm_Password)
                {
                    ViewBag.Message = "Password did not match!....";
                }
                else
                {
                    BAL obj1 = new BAL();
                    obj1.ChangePassword(password, id);
                }

            }
            

        }
        public string SelectUser()
        {
            string Email = string.Empty;
            HttpCookie rqCookies = Request.Cookies["UserInfo"];
            if(rqCookies != null)
            {
                Email = rqCookies["Email"];
            }
            BAL obj = new BAL();
            string userType = obj.GetUserTypeByEmail(Email);
            switch (userType)
            {
                case "admin":
                    return "admin";

                case "user":
                    return "user";

                default:
                    return "unknown";

            }

        }

    }
}

