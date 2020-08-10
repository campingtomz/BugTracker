using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
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
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //public async Task<ActionResult> Contact(EmailModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var body = "<p>Email From: <bold>{0}</bold>({1})</p><p>Message: </p><p>{2}</p>";
        //            var from = "MyPortfolio<example@email.com>";
        //            model.Body = "This is a message from your Blog site. The name and the email of the contact person ";
        //            var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
        //            {
        //                Subject = "Blog Contact Message",
        //                Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
        //                IsBodyHtml = true
        //            };
        //            var svc = new EmailService();
        //            await svc.SendAsync(email);

        //            return View(new EmailModel());
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            await Task.FromResult(0);
        //        }
        //    }
        //    return View(model);
        //}
    }
}