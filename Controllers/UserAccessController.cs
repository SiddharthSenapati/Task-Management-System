//using DataAccess;
using BusinessAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace First_MVC.Controllers
{
    public class UserAccessController : Controller
    {
        // GET: UserAccess
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddUser()
        {
            ViewBag.Message = "Passwords Should Match!..";
            return View();
        }
        [HttpPost]
        public ActionResult AddUserRegistration()
        {
            string UserName = Request["UserName"];
            string UserEmail = Request["UserEmail"];
            string UserType = Request["EType"];
            string UserPassword = Request["UserPassword"];
            string ConfirmPwd = Request["CPassword"];
            if (UserPassword == ConfirmPwd)
            {
                BAL obj = new BAL();
                obj.InsertUser(UserName, UserEmail, UserType, UserPassword);
                // LoggerClass.WriteEmp("A New Employee " + EmpName + " has been added");
                ViewBag.Message = "Successfully Added!....";
                LoggerClass.WriteUser("A new user " + UserName + " of type " + "[" + UserType + "] " + " has been added.");
            }
            else
            {
                Console.WriteLine("Password didnot match!...");
            }
            return RedirectToAction("UserList");
        }
        [HttpGet]
        public ActionResult UserList()
        {
            BAL obj = new BAL();
            DataSet ds = obj.GetUsersList();
            return View(ds);
        }
        public ActionResult Edituser(string id)
        {
            HttpCookie mycookie = new HttpCookie("mycookie");    //sending id in cookie
            mycookie["UserId"] = id;
            mycookie.Expires.Add(new TimeSpan(0, 1, 0));
            Response.Cookies.Add(mycookie);
            BAL obj = new BAL();
            string user = obj.GetUserNameByID(id);
            string[] array = user.Split(' ');
            string UserName = array[0];
            string UserType = array[1];
            LoggerClass.WriteUser("User " + UserName + "of type " + UserType + " has been Updated.");
            DataSet ds = obj.GetUsersByID(id);
            return View(ds);
        }
        [HttpPost]
        public ActionResult EditUserByID()
        {
            string UserName = Request["UserName"];
            string UserEmail = Request["UserEmail"];
            string UserPassword = Request["UserPassword"];
            string UserType = Request["UserType"];
            string CreationDate = Request["Created_at"];
            string id = string.Empty;
            HttpCookie reqcookies = Request.Cookies["mycookie"];
            if (reqcookies != null)
            {
                id = reqcookies["UserId"];
            }
            BAL obj = new BAL();
            obj.UpdateUser(UserName, UserEmail, UserPassword, UserType, CreationDate, id);
            return RedirectToAction("UserList");
        }
        [HttpGet]
        public ActionResult DeleteUser(string row)
        {
            BAL obj = new BAL();
            string user = obj.GetUserNameByID(row);
            string[] parts = user.Split(' ');
            string UserName = parts[0];
            string UserType = parts[1];
            LoggerClass.WriteUser("User " + UserName + " of type " + UserType + " has been Deleted.");
            obj.DeleteUser(row);
            return RedirectToAction("UserList");
        }
    }
}