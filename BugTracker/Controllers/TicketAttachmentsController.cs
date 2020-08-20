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
        private NotificationHelper notificationHelper = new NotificationHelper();

       

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFileToTicket([Bind(Include = "TicketId,FileName")] TicketAttachment ticketAttachments, string AttachmentDescription, HttpPostedFileBase file)
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
                    ticketAttachments.FileName = fileName;
                    db.TicketAttachments.Add(ticketAttachments);
                    db.SaveChanges();
                    notificationHelper.TicketNewAttachmentAdded(ticketAttachments);
                    return RedirectToAction("Dashboard", "Tickets", new { id = ticketAttachments.TicketId });
                }

            }

            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttahment.TicketId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttahment.UserId);
            TempData["Error"] = "Model Invalid";
            return RedirectToAction("Dashboard", "Tickets", new { id = ticketAttachments.TicketId });
        }

        // GET: TicketAttachments/Edit/5
       
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachments = db.TicketAttachments.Find(id);
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
            TicketAttachment ticketAttachments = db.TicketAttachments.Find(id);
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
