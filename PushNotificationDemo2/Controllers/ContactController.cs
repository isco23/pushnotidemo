using PushNotificationDemo2.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PushNotificationDemo2.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Get()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT [ContactName],[ContactNo] from [dbo].[Contacts]", connection))
                {
                    cmd.Notification = null;
                    SqlDependency dependency = new SqlDependency(cmd);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                    if(connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlDataReader reader = cmd.ExecuteReader();
                    var listEmp = reader.Cast<IDataRecord>().Select(x => new
                    {
                        ContactName = (string)x["ContactName"],
                        ContactNo = (string)x["ContactNo"],
                    }).ToList();
                    return Json(new { listEmp = listEmp }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        private void dependency_OnChange(object sender , SqlNotificationEventArgs e)
        {
            ContactHub.Show();
        }
    }
}