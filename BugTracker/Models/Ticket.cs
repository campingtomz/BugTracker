﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        #region Parents/Children
        public int ProjectId { get; set; }
        public int TicketPriorityId { get; set; }
        public int TicketStatusId { get; set; }
        public int TicketTypeId { get; set; }
        public string SubmitterId { get; set; }
        public string DeveloperId { get; set; }
        public virtual Project project { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual ApplicationUser Submitter { get; set; }
        public virtual ApplicationUser Developer { get; set; }
        public virtual ICollection<TicketAttachments> Attachments { get; set; }
        public virtual ICollection<TicketComment> Comments { get; set; }
        public virtual ICollection<TicketHistory> Histories { get; set; }
        public virtual ICollection<TicketNotification> Notifications { get; set; }
        #endregion

        #region Actual Properties
        public string Issue { get; set; }
        public  string IssueDiscription{ get; set; }
        public DateTime Created{ get; set; }
        public DateTime? Updated { get; set; }
        public bool IsResolved { get; set; }
        public bool IsArchived { get; set; }


        #endregion

        #region Constructor
        public Ticket()
        {
            Attachments = new HashSet<TicketAttachments>();
            Comments = new HashSet<TicketComment>();
            Histories = new HashSet<TicketHistory>();
            Notifications = new HashSet<TicketNotification>();

        }

        #endregion
    }
}