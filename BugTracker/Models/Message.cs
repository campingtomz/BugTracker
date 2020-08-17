using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Message
    {
        public int Id { get; set; }
        #region Parents/Children
        public int ConnectionId { get; set; }
        public virtual Connection Connection { get; set; }
        public string SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public string RecipientId { get; set; }
        public virtual ApplicationUser Recipient { get; set; }

        #endregion
        #region Actual Property
        public string Content { get; set; }
        public DateTime Created { get; set; }

        #endregion
    }
}