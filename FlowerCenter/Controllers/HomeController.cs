using FlowerCenter.Models.BLL;
using FlowerCenter.Models.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace FlowerCenter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
       {
            ViewBag.itemList = new itemService().GetAll();
            ViewBag.Featureditems = new itemService().GetAllFeatured();
            ViewBag.Category = new categoryBLL().GetAll();
            //ViewBag.SubCategory = new subcategoryBLL().GetAll();
            //ViewBag.Color = new colorBLL().GetAll();
            ViewBag.Deal = new dealBLL().GetAll();
            ViewBag.TenItems = new itemService().GetSelecteditems(); 
            ViewBag.Banner = new bannerBLL().GetBanner("Home");
            ViewBag.Reviews= new bannerBLL().GetReviews();
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Banner = new bannerBLL().GetBanner("About");
            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Banner = new bannerBLL().GetBanner("Contact");
            return View();
        }
        [HttpPost]
        public ActionResult Contact(contactBLL obj)
        {
            ViewBag.Contact = "";
            string ToEmail, SubJect, cc, Bcc;
            cc = "";
            Bcc = "";
            ToEmail = ConfigurationManager.AppSettings["From"].ToString();
            SubJect = "New Query From Customer";
            string BodyEmail = System.IO.File.ReadAllText(Server.MapPath("~/Template") + "\\" + "contact.txt");
            DateTime dateTime = DateTime.UtcNow.Date;
            BodyEmail = BodyEmail.Replace("#Date#", dateTime.ToString("dd/MMM/yyyy"))
            .Replace("#Name#", obj.Name.ToString())
            .Replace("#Email#", obj.Email.ToString())
            .Replace("#Contact#", obj.Phone.ToString())
            .Replace("#Subject#", obj.Subject.ToString())
            .Replace("#Message#", obj.Message.ToString());
            try
            {
                //WebMail.SmtpServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                //WebMail.SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                //WebMail.SmtpUseDefaultCredentials = true;
                //WebMail.EnableSsl = false;
                //WebMail.UserName = ConfigurationManager.AppSettings["UserName"].ToString();

                //WebMail.From = ConfigurationManager.AppSettings["From"].ToString();
                //WebMail.Password = ConfigurationManager.AppSettings["Password"].ToString();
                //WebMail.Send(to: ToEmail, subject: SubJect, body: Body, cc: cc, bcc: Bcc, isBodyHtml: true);
                MailMessage mail = new MailMessage();
                mail.To.Add(ToEmail);
                mail.From = new MailAddress(ConfigurationManager.AppSettings["From"].ToString());
                mail.Subject = SubJect;
                string Body = BodyEmail;
                mail.Body = Body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                smtp.Host = ConfigurationManager.AppSettings["SmtpServer"].ToString(); //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential
                     (ConfigurationManager.AppSettings["From"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                //Or your Smtp Email ID and Password
                smtp.EnableSsl = true;

                smtp.Send(mail);
                ViewBag.Contact = "Your Query is received. Our support department contact you soon.";
            }
            catch (Exception ex)
            {
                ViewBag.Contact = "";
            }
            return View();
        }
        public JsonResult Subscribe(string email)
        {
            string ToEmail, SubJect, cc, Bcc;
            cc = "";
            Bcc = "";
            ToEmail = ConfigurationManager.AppSettings["From"].ToString();
            SubJect = "New Subscribtion at FlowerCenter";
            string BodyEmail = System.IO.File.ReadAllText(Server.MapPath("~/Template") + "\\" + "newsletter.txt");
            BodyEmail = BodyEmail.Replace("#email#", email.ToString());
            try
            {
                //WebMail.SmtpServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                //WebMail.SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                //WebMail.SmtpUseDefaultCredentials = true;
                //WebMail.EnableSsl = false;
                //WebMail.UserName = ConfigurationManager.AppSettings["UserName"].ToString();

                //WebMail.From = ConfigurationManager.AppSettings["From"].ToString();
                //WebMail.Password = ConfigurationManager.AppSettings["Password"].ToString();
                //WebMail.Send(to: ToEmail, subject: SubJect, body: Body, cc: cc, bcc: Bcc, isBodyHtml: true);
                MailMessage mail = new MailMessage();
                mail.To.Add(ToEmail);
                mail.From = new MailAddress(ConfigurationManager.AppSettings["From"].ToString());
                mail.Subject = SubJect;
                
                mail.Body = BodyEmail;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                smtp.Host = ConfigurationManager.AppSettings["SmtpServer"].ToString(); //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential
                     (ConfigurationManager.AppSettings["From"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                //Or your Smtp Email ID and Password
                smtp.EnableSsl = true;

                smtp.Send(mail);
                ViewBag.Status = "";
            }
            catch (Exception ex)
            {
                ViewBag.Status = "";
            }

            return Json(email, JsonRequestBehavior.AllowGet);
        }
        //Get Setting Details
        public ActionResult GetSetting()
        {
            return Json(new settingBLL().GetSettings(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Policy()
        {
            return View();
        }
    }
}