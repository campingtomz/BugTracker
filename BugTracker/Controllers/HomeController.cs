using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserHelper userHelper = new UserHelper();
        private UserRoleHelper roleHelper = new UserRoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private HistoryHelper historyHelper = new HistoryHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private HomeViewHelper homeHelper = new HomeViewHelper();
        public ActionResult Index()
        {
            var user = userHelper.getUser(User.Identity.GetUserId());
            var model = new HomeVM();
            model.UserId = user.Id;
            model.User = user;
            model.Tickets = ticketHelper.GetMyTickets().ToList();
            model.TotalNotificationsCount = user.TicketNotifications.Count + user.ProjectNotifications.Count;
            model.UsersOnMyProjects = projectHelper.GetUsersOnMyProjects();
            List<int> ProjectIds = new List<int>();

            foreach(var project in projectHelper.GetUserProjects())
            {
                ProjectIds.Add(project.Id);
            }
            model.ProjectIds = ProjectIds;
            return View(model);
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
        public ActionResult Chat()
        {
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