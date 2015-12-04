using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ASPNET.Models;
using System.Net.Mail;
using System.Text;
using System.Configuration;

namespace ASPNET.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Title = "Contact Us";
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactModels c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msg = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    MailAddress from = new MailAddress(c.Email.ToString());
                    StringBuilder sb = new StringBuilder();

                    msg.To.Add(ConfigurationManager.AppSettings.Get("supportEmail"));
                    msg.Subject = "Contact Us";
                    msg.IsBodyHtml = false;
                    //UNCOMMENT IF YOU ARE NOT SETTING IT IN THE WEB.CONFIG
                    //smtp.Host = "mail.yourdomain.com";
                    //smtp.Port = 25;
                    sb.Append("First name: " + c.FirstName);
                    sb.Append(Environment.NewLine);
                    sb.Append("Last name: " + c.LastName);
                    sb.Append(Environment.NewLine);
                    sb.Append("Email: " + c.Email);
                    sb.Append(Environment.NewLine);
                    sb.Append("Comments: " + c.Comment);
                    msg.Body = sb.ToString();
                    smtp.Send(msg);
                    msg.Dispose();
                    return View("ContactSuccess");
                }
                catch (Exception e)
                {
                    var message = e.Message;
                    return View("Error");
                }
            }
            return View();
        }
        public ActionResult ContactSuccess()
        {
            ViewBag.Title = "Message Sent";
            ViewBag.Message = "Your message has been sent.";

            return View();
        }
    }
}