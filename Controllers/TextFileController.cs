using First_MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

public class TextFileController : Controller
{
    [HttpGet]
    public ActionResult ShowTextFile()
    {
        string filePath = Path.Combine(Server.MapPath("~/LogFile/mvcLog.txt"));

        List<LogProject> logProjects = new List<LogProject>();


        foreach (string line in System.IO.File.ReadLines(filePath))
        {
            string[] parts = line.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime timestamp = DateTime.Parse(parts[0]);

            string action = parts[1];

            LogProject record = new LogProject
            {
                TimeStamp = timestamp,
                //Action = action,
                content = action,
                // EmployeeID = employeeID
            };

            logProjects.Add(record);
        }

        // Pass logRecords to the view
        return View(logProjects);
    }
    [HttpGet]
    public ActionResult ShowTaskHistory()
    {
        string filePath = Path.Combine(Server.MapPath("~/LogFile/mvcTask.txt"));
        List<LogTask> logtasks = new List<LogTask>();
        //PrintAllEmployee(filePath);


        foreach (string line in System.IO.File.ReadLines(filePath))
        {
            string[] parts = line.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime timestamp = DateTime.Parse(parts[0]);

            string action = parts[1];

            LogTask record = new LogTask
            {
                TimeStamp = timestamp,
                //Action = action,
                Content = action,
                // EmployeeID = employeeID
            };

            logtasks.Add(record);
        }

        // Pass logRecords to the view
        return View(logtasks);
    }
    public ActionResult ShowEmpHistory()
    {
        string filePath = Path.Combine(Server.MapPath("~/LogFile/mvcEmp.txt"));

        List<LogEmp> logEmps = new List<LogEmp>();


        foreach (string line in System.IO.File.ReadLines(filePath))
        {
            string[] parts = line.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime timestamp = DateTime.Parse(parts[0]);

            string action = parts[1];

            LogEmp record = new LogEmp
            {
                TimeStamp = timestamp,
                //Action = action,
                content = action,
                // EmployeeID = employeeID
            };

            logEmps.Add(record);
        }

        // Pass logRecords to the view
        return View(logEmps);
    }
    public ActionResult ShowUserHistory()
    {
        string filePath = Path.Combine(Server.MapPath("~/LogFile/mvcUser.txt"));
        List<LogRecord> logRecords = new List<LogRecord>();

        // Read each line and parse it
        foreach (string line in System.IO.File.ReadLines(filePath))
        {
            string[] parts = line.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime timestamp = DateTime.Parse(parts[0]);

            //string[] actionAndEmployee = parts[3].Trim().Split(' ');

            string action = parts[1];

            //string employeeName = string.Join(" ", actionAndEmployee.Skip(2));

            int? employeeID = null;

            //if (action.Length >= 5)// && //actionAndEmployee[actionAndEmployee.Length - 2] == "ID")
            //{
            //    string idString = actionAndEmployee[actionAndEmployee.Length - 1].Trim(new char[] { '[', ']' });
            //    int id;
            //    if (int.TryParse(idString, out id))
            //    {
            //        employeeID = id;
            //    }
            //}

            LogRecord record = new LogRecord
            {
                Timestamp = timestamp,
                //Action = action,
                EmployeeName = action,
                EmployeeID = employeeID
            };

            logRecords.Add(record);
        }

        // Pass logRecords to the view
        return View(logRecords);

    }
    //public ActionResult PrintAllEmployee()
    //{
    //    var report = new Rotativa.ActionAsPdf("ShowTaskHistory");
    //    return report;
    //}


    public ActionResult PrintAllEmployee()
    {
        // Read the content of the text file
        string filePath = Path.Combine(Server.MapPath("~/LogFile/mvcTask.txt")); // Provide the path to your text file
        string fileContent = System.IO.File.ReadAllText(filePath);

        // Pass the file content to the view
        return new Rotativa.ViewAsPdf("ShowTaskHistory", (object)fileContent);
    }

}
