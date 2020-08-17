using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class MessagesController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ChatMessages
        //public ActionResult Index()
        //{
        //    var chatMessages = db.Messages.Include(c => c.User).Include(c => c.UserId);
        //    return View(chatMessages.ToList());
        //}

        // GET: ChatMessages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message chatMessage = db.Messages.Find(id);
            if (chatMessage == null)
            {
                return HttpNotFound();
            }
            return View(chatMessage);
        }

        // POST: ChatMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserChatId, Message")] Message chatMessage, string recipientId)
        {
            if (ModelState.IsValid)
            {
                chatMessage.SenderId = User.Identity.GetUserId();
                chatMessage.RecipientId = recipientId;
                chatMessage.Created = DateTime.Now;
                db.Messages.Add(chatMessage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: ChatMessages/Edit/5

        // GET: ChatMessages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message chatMessage = db.Messages.Find(id);
            if (chatMessage == null)
            {
                return HttpNotFound();
            }
            return View(chatMessage);
        }

        // POST: ChatMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message chatMessage = db.Messages.Find(id);
            db.Messages.Remove(chatMessage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
