using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class TicketAttahmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttahments
        public ActionResult Index()
        {
            var ticketAttahments = db.TicketAttahments.Include(t => t.ticket).Include(t => t.User);
            return View(ticketAttahments.ToList());
        }

        // GET: TicketAttahments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttahment ticketAttahment = db.TicketAttahments.Find(id);
            if (ticketAttahment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttahment);
        }

        // GET: TicketAttahments/Create
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketAttahments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,UserId,FilePath,Description,Created")] TicketAttahment ticketAttahment)
        {
            if (ModelState.IsValid)
            {
                db.TicketAttahments.Add(ticketAttahment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttahment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttahment.UserId);
            return View(ticketAttahment);
        }

        // GET: TicketAttahments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttahment ticketAttahment = db.TicketAttahments.Find(id);
            if (ticketAttahment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttahment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttahment.UserId);
            return View(ticketAttahment);
        }

        // POST: TicketAttahments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,UserId,FilePath,Description,Created")] TicketAttahment ticketAttahment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttahment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttahment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttahment.UserId);
            return View(ticketAttahment);
        }

        // GET: TicketAttahments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttahment ticketAttahment = db.TicketAttahments.Find(id);
            if (ticketAttahment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttahment);
        }

        // POST: TicketAttahments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttahment ticketAttahment = db.TicketAttahments.Find(id);
            db.TicketAttahments.Remove(ticketAttahment);
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
