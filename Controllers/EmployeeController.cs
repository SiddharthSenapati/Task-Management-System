using BusinessAccess;
//using DataAccess;
using First_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace First_MVC.Controllers
{
    public class EmployeeController : Controller
    {
        //private readonly YourDbContext _context;
        [HttpGet]
        public ActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeRegistration()
        {
            string EmpName = Request["EmpName"];
            string EmpEmail = Request["EmpEmail"];
            string EmpType = Request["EType"];
            string pwd = Request["EmpPassword"];
            BAL obj = new BAL();
            obj.InsertEmployee(EmpName, EmpEmail);
            obj.InsertUser(EmpName, EmpEmail, EmpType, pwd);
            LoggerClass.WriteEmp("A New Employee " + EmpName + " has been added");
            ViewBag.Message = "Successfully Added!....";
            return RedirectToAction("EmployeeList");
        }
        [HttpGet]
        public ActionResult EmployeeList()
        {
            DataSet ds = new DataSet();
            BAL obj = new BAL();
            ds = obj.GetEmpList();
            return View(ds);
            //List<Employee> list = new List<Employee>();
            //DAL objDal = new DAL();
            ////list = objDal.GetAllEmployee();
            //ViewBag.model = list;
            //return View();
        }

        [HttpGet]
        public ActionResult Edit(String id)
        {
            HttpCookie myCookie = new HttpCookie("myCookie");       //sending Cookies 

            myCookie["EmpID"] = id;
            myCookie.Expires.Add(new TimeSpan(0, 1, 0));
            Response.Cookies.Add(myCookie);
            BAL obj = new BAL();
            Employee objEmp = new Employee();
            DataSet dt = obj.GetEmpDetailsByID(id);
            TempData["msg"] = "Successfully changed!...";
            ViewBag.Message = "Successfully changed!...";
            return View(dt);

        }
        [HttpPost]
        public ActionResult PostByRequest()
        {
            string EmpID = string.Empty;
            string EmpName = Request["EmpName"];
            string EmpEmail = Request["EmpEmail"];
            HttpCookie reqCookies = Request.Cookies["myCookie"];
            if (reqCookies != null)
            {
                EmpID = reqCookies["EmpID"];        //Accessing The Cookies
            }

            //string EmpID = TempData["EmpID"].ToString();
            BAL obj = new BAL();
            obj.UpdateEmp(EmpName, EmpEmail, EmpID);
            LoggerClass.WriteEmp("Employee Named " + EmpName + " having ID " + EmpID + " has been updated ");
            ViewBag.Message = "Successfully changed!...";
            TempData["msg"] = "Successfully changed!...";
            return RedirectToAction("EmployeeList");

        }

        [HttpGet]
        public ActionResult Delete(string row)
        {
            HttpCookie myCookie = new HttpCookie("myCookie");       //sending Cookies 

            myCookie["ID"] = row;
            myCookie.Expires.Add(new TimeSpan(0, 1, 0));
            Response.Cookies.Add(myCookie);
            BAL obj = new BAL();
            string EmpName = obj.DeleteEmp(row);
            LoggerClass.WriteEmp("Employee " + EmpName + " having ID [" + row + "] was deleted.");
            ViewBag.Msg = "Successfully Deleted!...";
            return RedirectToAction("EmployeeList");
        }

    }

}