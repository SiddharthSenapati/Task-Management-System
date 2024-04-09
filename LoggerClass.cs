using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace First_MVC
{
    public class LoggerClass
    {
        public static void WriteLog(string message)
        {
            string logPath = ConfigurationManager.AppSettings["logPath"];  //for project

            using (StreamWriter writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine($"{DateTime.Now} || {message}");
            }
        }
        public static void Write(string message)
        {
            string logpath2 = ConfigurationManager.AppSettings["logPath2"];   //for Task
            using (StreamWriter strw = new StreamWriter(logpath2, true))
            {
                strw.WriteLine($"{DateTime.Now} || {message}");
            }
        }
        public static void WriteEmp(string message)
        {
            string logpath3 = ConfigurationManager.AppSettings["logPath3"];  //for Employees
            using (StreamWriter strw = new StreamWriter(logpath3, true))
            {
                strw.WriteLine($"{DateTime.Now} || {message}");
            }
        }
        public static void WriteUser(string message)
        {
            string logpath4 = ConfigurationManager.AppSettings["logPath4"];   //for Users
            using (StreamWriter strw = new StreamWriter(logpath4, true))
            {
                strw.WriteLine($"{DateTime.Now} || {message}");
            }
        }
    }
}