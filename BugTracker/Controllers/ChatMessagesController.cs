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
    public class ChatMessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ChatMessages
        public ActionResult Index()
        {
            var chatMessages = db.ChatMessages.Include(c => c.User).Include(c => c.userChat);
            return View(chatMessages.ToList());
        }

        // GET: ChatMessages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatMessage chatMessage = db.ChatMessages.Find(id);
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
        public ActionResult Create([Bind(Include = "UserChatId,Message")] ChatMessage chatMessage)
        {
            if (ModelState.IsValid)
            {
                chatMessage.UserId = User.Identity.GetUserId();
                chatMessage.Created = DateTime.Now;
                db.ChatMessages.Add(chatMessage);
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
            ChatMessage chatMessage = db.ChatMessages.Find(id);
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
            ChatMessage chatMessage = db.ChatMessages.Find(id);
            db.ChatMessages.Remove(chatMessage);
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
