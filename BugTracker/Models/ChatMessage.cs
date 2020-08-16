using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        #region Parents/Children
        public int UserChatId { get; set; }
        public virtual UserChat userChat { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        #endregion
        #region Actual Property
        public string Message { get; set; }
        public DateTime Created { get; set; }

        #endregion
    }
}