//using DataAccess;
using BusinessAccess;
using First_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace First_MVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly string certificatesFolderPath = "/Attachments/Files/";
        [HttpGet]
        public ActionResult ProjectAdd()
        {
            
            return View();
        }
        [HttpGet]
        public ActionResult Task()
        {
            Project Pname = new Project();
            Pname.Projects = PopulateNames();
            Pname.EmployeesList = PopulateEmp();
            return View(Pname); 
        }
        [HttpPost]
        public ActionResult Task(Project Pname)
        {
            Pname.Projects = PopulateNames();     //Adding Projects
            var selectedItem = Pname.Projects.Find(p => p.Value == Pname.ProjectID.ToString());

            Pname.EmployeesList = PopulateEmp();    //Adding Employees
            var selectedEmp = Pname.EmployeesList.Find(p => p.Value == Pname.EmpID.ToString());
            return View(Pname);

        }
        private static List<SelectListItem> PopulateNames()
        {
            List<SelectListItem> items = new List<SelectListItem>();
           string conn = ConfigurationManager.ConnectionStrings["RegForm"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conn))
            {
               
                SqlCommand cmd = new SqlCommand("Select ProjectID, ProjectName from ProjectSidd", con);
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = sdr["ProjectName"].ToString(),
                        Value = sdr["ProjectID"].ToString()
                    });
                }
                con.Close();
                return items;
            }
        }
        private static List<SelectListItem> PopulateEmp()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string conn = ConfigurationManager.ConnectionStrings["RegForm"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conn))
            {

                SqlCommand cmd = new SqlCommand("Select EmpID, EmpName from EmpSidd", con);
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = sdr["EmpName"].ToString(),
                        Value = sdr["EmpID"].ToString()
                    });
                }
                con.Close();
                return items;
            }
        }

        [HttpPost]
        public ActionResult PostByValue(HttpPostedFileBase fileUpload)                              //for adding task
        {
            string ProjectID = Request["ProjectID"];
            string TaskName = Request["TaskName"];
            string TaskDesc = Request["TaskDescription"];
            string startDate = Request["projectStart"];
            string endDate = Request["projectEnd"];
            string taskChanges = Request["txtchangebox"];
            //string EmpName = Request["EmpName"];
            string EmpID = Request["EmpID"];
            //string file = Request["file"];
            string filePath = null;
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                filePath = SaveFile(fileUpload, certificatesFolderPath);
            }

            BAL obj = new BAL();
            obj.insertTask(TaskName, ProjectID, TaskDesc, startDate, endDate, taskChanges, filePath, EmpID);
           
            //obj.sendEmailForTaskCreation();
            ViewBag.Message = "Task Successfully Added!....";
            string ProjectName = obj.GetProjectNameByID(ProjectID);
            obj.Send_Mail_ToEmp(TaskName, ProjectName,TaskDesc,startDate,endDate,EmpID);
            LoggerClass.Write(TaskName +" " + "belongs to " + ProjectName + " is created.");

            return RedirectToAction("TaskList");
        }
        public ActionResult PostByForm(FormCollection form, HttpPostedFileBase fileUpload)          //for adding Project
        {
            string projectName = form["Pname"];
            string projectDesc = form["PDesc"];
            string projectStart = form["projectStart"];
            string projectEnd = form["projectEnd"];
            //string file = form["file"];
            string filePath = null;
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                filePath = SaveFile(fileUpload, certificatesFolderPath);
            }

            BAL obj = new BAL();
            obj.insertProject(projectName, projectDesc, projectStart, projectEnd, filePath);
            ViewBag.Message = "Project Successfully Added!....";
            LoggerClass.WriteLog(projectName + " " + " is created");
            return RedirectToAction("ProjectList");
        }
        protected string SaveFile(HttpPostedFileBase fileUpload, string folderPath)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileUpload.FileName);
            string filePath = Server.MapPath($"{folderPath}{fileName}");
            fileUpload.SaveAs(filePath);
            return $"{folderPath}{fileName}";
        }
        [HttpGet]
        public ActionResult TaskList()
        {
            DataSet ds = new DataSet();
            BAL obj = new BAL();
            ds = obj.GetTaskList();
            return View(ds);
        }
        [HttpGet]
        public ActionResult EditTask(String id)
        {
            HttpCookie myCookie = new HttpCookie("myCookie");       //sending Cookies 

            myCookie["TID"] = id;
            myCookie.Expires.Add(new TimeSpan(0, 1, 0));
            Response.Cookies.Add(myCookie);
            BAL obj = new BAL();
            DataSet dt = obj.GetTaskDetailsByID(id);
            TempData["msg"] = "Successfully changed!...";
            ViewBag.Message = "Successfully changed!...";
            return View(dt);

        }
        [HttpPost]
        public ActionResult EditTaskByID(HttpPostedFileBase fileUpload)
        {
            string TaskName = Request["TaskName"];
            string ProjectName = Request["ProjectName"];
            string TaskDesc = Request["TaskDesc"];
            string TaskChanges = Request["TaskChanges"];
            //string TaskAttachment = Request["TaskAttachment"];
            string AssignedTo = Request["AssignedTo"];
            string StartDate = Request["StartDate"];
            string EndDate = Request["EndDate"];

            string ID = string.Empty;
            HttpCookie reqCookies = Request.Cookies["myCookie"];
            if (reqCookies != null)
            {
                ID = reqCookies["TID"];        //Accessing The Cookies
            }
            string filePath = null;
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                filePath = SaveFile(fileUpload, certificatesFolderPath);
            }
            BAL obj = new BAL();
            obj.Send_Mail_ToEmp(TaskName, ProjectName, TaskDesc, StartDate, EndDate, ID);
            obj.UpdateTask(TaskName, ProjectName, TaskDesc, TaskChanges, filePath, AssignedTo, StartDate, EndDate, ID);
            LoggerClass.Write("Task Name" + " " + TaskName + " " + " of Project" + ProjectName + " " + "is Updated.");
            return RedirectToAction("TaskList");
        }
        [HttpGet]
        public ActionResult DeleteTask(string row)
        {
            BAL obj = new BAL();
            String name = obj.GetTaskNameProjectNameByTaskID(row);
            string[] parts = name.Split(' ');
            string TaskName = parts[0];
            string ProjectName = parts[1];
            LoggerClass.Write("Task Name" + " " + TaskName + " " + " of Project " + ProjectName + " " + "is Deleted.");
            obj.DeleteTaskByID(row);
            return RedirectToAction("TaskList");
        }
        public ActionResult ProjectList()
        {
            BAL obj = new BAL();
            DataSet ds = obj.GetProjectDetails();
            return View(ds);
        }
        public ActionResult EditProject(string id)
        {
            HttpCookie myCookie = new HttpCookie("myCookie");       //sending Cookies 

            myCookie["PID"] = id;
            myCookie.Expires.Add(new TimeSpan(0, 1, 0));
            Response.Cookies.Add(myCookie);
            BAL obj = new BAL();
            DataSet ds =  obj.GetProjectDetailsByID(id);
            TempData["msg"] = "Successfully changed!...";
            ViewBag.Message = "Successfully changed!...";
            //string projectName = string.Empty;
            //string projectID = string.Empty;
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    projectID = ds.Tables[0].Rows[0][0].ToString();
            //    projectName = ds.Tables[0].Rows[0][1].ToString();
            //}
            //LoggerClass.WriteLog("projectID" + "[" + projectID +"]"+ projectName+ " " + " is Updated");
            return View(ds);
        }
        [HttpPost]
        public ActionResult EditProjectByID(HttpPostedFileBase fileUpload)
        {
            string ProjectName = Request["ProjectName"];
            string ProjectDesc = Request["ProjectDesc"];
            string StartDate =   Request["StartDate"];
            string EndDate   = Request["EndDate"];

            string ProjectID = string.Empty;
            HttpCookie reqCookies = Request.Cookies["myCookie"];
            if (reqCookies != null)
            {
                ProjectID = reqCookies["PID"];        //Accessing The Cookies
            }
            string filePath = null;
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                filePath = SaveFile(fileUpload, certificatesFolderPath);
            }
            BAL obj = new BAL();
            obj.EditProjectByID(ProjectName, ProjectDesc, filePath, StartDate, EndDate, ProjectID);
            string projectName = obj.GetProjectNameByID(ProjectID);
            LoggerClass.WriteLog("ProjectID" + "[" + ProjectID + "]" +"--"+"Project Name"+" "+ projectName + " " + " is Updated");
            return RedirectToAction("ProjectList");
        }
        [HttpGet]
        public ActionResult DeleteProject(string row)
        {
            BAL obj = new BAL();
            obj.DeleteProjectByID(row);
            string projectName = obj.GetProjectNameByID(row);

            LoggerClass.WriteLog("ProjectID" + "[" + row + "]" + projectName + " " + " is Deleted");
            return RedirectToAction("ProjectList");
        }
        
    }
}