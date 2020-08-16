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
using System.Web.Configuration;
using System.IO;

namespace BugTracker.Controllers
{
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        public ActionResult Index()
        {
            var ticketAttachments = db.TicketAttachments.Include(t => t.ticket).Include(t => t.User);
            return View(ticketAttachments.ToList());
        }

        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachments ticketAttachments = db.TicketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Create
        //public ActionResult UploadFileToTicket()
        //{
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId");
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
        //    return View();
        //}

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFileToTicket([Bind(Include = "TicketId,FileName")] TicketAttachments ticketAttachments, string AttachmentDescription, HttpPostedFileBase file)
        {
            if (file == null)
            {
                TempData["Error"] = "You must Supply a vaild File";
                return RedirectToAction("Dashboard", "Tickets", new { id = ticketAttachments.TicketId });
            }
            if (ModelState.IsValid)
            {


                if (FileUploadValidator.IsWebFriendlyImage(file) || FileUploadValidator.IsWebFriendlyFile(file))
                {
                    ticketAttachments.Created = DateTime.Now;
                    ticketAttachments.UserId = User.Identity.GetUserId();
                    ticketAttachments.Description = AttachmentDescription;

                    var fileName = FileStamp.MakeUnique(file.FileName);
                    var serverFolder = WebConfigurationManager.AppSettings["DefaultAttachmentFolder"];
                    file.SaveAs(Path.Combine(Server.MapPath(serverFolder), fileName));
                    ticketAttachments.FilePath = $"{serverFolder}{fileName}";

                    db.TicketAttachments.Add(ticketAttachments);
                    db.SaveChanges();
                    return RedirectToAction("Dashboard", "Tickets", new { id = ticketAttachments.TicketId });
                }

            }

            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttahment.TicketId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttahment.UserId);
            TempData["Error"] = "Model Invalid";
            return RedirectToAction("Dashboard", "Tickets", new { id = ticketAttachments.TicketId });
        }

        // GET: TicketAttachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachments ticketAttachments = db.TicketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttachments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachments.UserId);
            return View(ticketAttachments);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,UserId,FilePath,Description,Created")] TicketAttachments ticketAttachments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttachments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachments.UserId);
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachments ticketAttachments = db.TicketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachments);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachments ticketAttachments = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachments);
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
