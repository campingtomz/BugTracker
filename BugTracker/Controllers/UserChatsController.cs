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
using BugTracker.Helpers;

namespace BugTracker.Controllers
{
    public class UserChatsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserHelper userHelper = new UserHelper();
        ChatHelper chatHelper = new ChatHelper();
        

        // GET: UserChats
        public ActionResult Index()
        {
            var user = userHelper.getUser(User.Identity.GetUserId());
            return View(user.UserChats.ToList());
            
        }

        // GET: UserChats/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserChat userChat = db.UserChats.Find(id);
        //    if (userChat == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userChat);
        //}

        // GET: UserChats/Create
      

        // POST: UserChats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( UserChat userChat, String User2Id)
        {
            if (ModelState.IsValid)
            {
                //if (chatHelper.chatExists(User2Id))
                //{
                //    userChat.Created = DateTime.Now;
                //    userChat.UsersId.Add(User.Identity.GetUserId());
                //    userChat.UsersId.Add(User2Id);
                //    db.UserChats.Add(userChat);
                //    db.SaveChanges();
                    
                //}
            } 
            return View();
        }
        // GET: UserChats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserChat userChat = db.UserChats.Find(id);
            if (userChat == null)
            {
                return HttpNotFound();
            }
            return View(userChat);
        }

        // POST: UserChats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserChat userChat = db.UserChats.Find(id);
            db.UserChats.Remove(userChat);
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
