//using DataAccess;
using BusinessAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace First_MVC.Controllers
{
    public class UserTaskController : Controller
    {
        // GET: UserTask
        private readonly string certificatesFolderPath = "/Attachments/Files/";
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult TaskListUser()
        {
            BAL obj = new BAL();
            string email = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                email = reqCookies["Email"];        //Accessing The Cookies
            }
            DataSet ds = obj.TaskListByEmpID(email);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                ViewBag.Message = "No tasks assigned";
            }
            else
            {
                string name = string.Empty;
                HttpCookie reqNameCookie = Request.Cookies["nameCookie"];
                if (reqNameCookie != null)
                {
                    name = reqNameCookie["UserName"];
                }
                ViewBag.Name = name;
            }
            return View(ds);
        }
        public ActionResult InvolvedProjects()
        {
            string email = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                email = reqCookies["Email"];        //Accessing The Cookies
            }
            BAL obj = new BAL();
            DataSet ds = obj.TaskListByEmpID(email);
            return View(ds);
        }
        [HttpGet]
        public ActionResult ForwardUser(string id)
        {
            HttpCookie TaskIDCookie = new HttpCookie("TaskIDCookie");       //sending Cookies
            TaskIDCookie["TaskID"] = id;
            TaskIDCookie.Expires.Add(new TimeSpan(10, 1, 1));
            Response.Cookies.Add(TaskIDCookie);

            string email = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                email = reqCookies["Email"];        //Accessing The Cookies
            }
            BAL obj = new BAL();
            string name = obj.GetUserNameByEmail(email);
            ViewBag.msg = name;
            DataSet ds = obj.GetTaskDetailsByID(id);
            return View(ds);
        }
        [HttpPost]
        public ActionResult ForwardUser(HttpPostedFileBase fileUpload)
        {
            string TaskID = string.Empty;
            HttpCookie reqTaskIDCookie = Request.Cookies["TaskIDCookie"];               //Accessing The Cookies      
            if (reqTaskIDCookie != null)
            {
                TaskID = reqTaskIDCookie["TaskID"];
            }
            string TaskName = Request["TaskName"];
            string ProjectName = Request["ProjectName"];
            string TaskDesc = Request["TaskDesc"];
            string TaskChanges = Request["TaskChanges"];
            string Assigned_To = Request["AssignedTo"];

            string filePath = null;
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                filePath = SaveFile(fileUpload, certificatesFolderPath);
            }
            //string fileUpload = Request["fileUpload"];
            string StartDate = Request["StartDate"];
            string EndDate = Request["EndDate"];

            BAL obj = new BAL();
            obj.InsertForwardTable(TaskID, TaskName, ProjectName, TaskDesc, TaskChanges, Assigned_To, filePath, StartDate, EndDate);
            return RedirectToAction("TaskListUser");
        }
        protected string SaveFile(HttpPostedFileBase fileUpload, string folderPath)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileUpload.FileName);
            string filePath = Server.MapPath($"{folderPath}{fileName}");
            fileUpload.SaveAs(filePath);
            return $"{folderPath}{fileName}";
        }
        [HttpGet]
        public ActionResult ForwardedTasks()
        {
            string email = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                email = reqCookies["Email"];        //Accessing The Cookies
            }
            BAL obj = new BAL();
            string EmpName = obj.GetEmpNameByEmail(email);
            ViewBag.EmpName = EmpName;
            string name = obj.GetUserNameByEmail(email);
            HttpCookie reqNameCookie = Request.Cookies["nameCookie"];
            

            DataSet ds = obj.GetDetailsFromForwardTable(name);
            return View(ds);
        }
        [HttpGet]
        public ActionResult ForwardAdmin(string id)
        {
            BAL obj = new BAL();
            DataSet ds = obj.GetForwardDetailsByIDToAdmin(id);
            return View(ds);
        }
    }
}