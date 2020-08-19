using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class ProjectNotification : Notification
    {
        public int Id { get; set; }
        #region Parents/Children
        public int ProjectId{ get; set; }
        public virtual Project Project { get; set; }
       
        #endregion
    }
}