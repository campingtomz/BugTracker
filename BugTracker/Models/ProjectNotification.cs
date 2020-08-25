using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class ProjectNotification 
    {
        public int Id { get; set; }
        #region Parents/Children
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        #endregion
        #region Actual Property
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public bool IsRead { get; set; }

        #endregion

    }
}