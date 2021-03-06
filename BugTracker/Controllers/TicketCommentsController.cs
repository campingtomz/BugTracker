﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Helpers;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private NotificationHelper notificationHelper = new NotificationHelper();

        // GET: TicketComments
        //public ActionResult Index()
        //{
        //    var ticketComments = db.TicketComments.Include(t => t.ticket).Include(t => t.User);
        //    return View(ticketComments.ToList());
        //}

        // GET: TicketComments/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketComment ticketComment = db.TicketComments.Find(id);
        //    if (ticketComment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticketComment);
        //}

        // GET: TicketComments/Create
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId,Comment")] TicketComment ticketComment)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrWhiteSpace(ticketComment.Comment))
                {
                    ticketComment.Created = DateTime.Now;
                    ticketComment.UserId = User.Identity.GetUserId();

                    db.TicketComments.Add(ticketComment);
                    db.SaveChanges();
                    notificationHelper.TicketNewCommentAdded(ticketComment);
                    return RedirectToAction("Dashboard", "Tickets", new { id = ticketComment.TicketId });
                }
            }

            TempData["Error"] = "Model Invalid";
            return RedirectToAction("Dashboard", "Tickets", new { id = ticketComment.TicketId });
        }
       
        // GET: TicketComments/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketComment ticketComment = db.TicketComments.Find(id);
        //    if (ticketComment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketComment.TicketId);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketComment.UserId);
        //    return View(ticketComment);
        //}

        //// POST: TicketComments/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,TicketId,UserId,Comment,Created")] TicketComment ticketComment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(ticketComment).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketComment.TicketId);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketComment.UserId);
        //    return View(ticketComment);
        //}

        // GET: TicketComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            return View(ticketComment);
        }

        // POST: TicketComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketComment ticketComment = db.TicketComments.Find(id);
            db.TicketComments.Remove(ticketComment);
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
