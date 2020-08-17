using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Connection
    {
        public int Id { get; set; }
        #region Parents/Children
        //public List<string> UserIds { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        //public string User1Id { get; set; }
        //public virtual ApplicationUser User1 { get; set; }
        //public string User2Id { get; set; }
        //public virtual ApplicationUser User2 { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        #endregion
        #region Actual Properties
        public bool Connected { get; set; }
        public bool isArchived { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }


        #endregion
        public Connection()
        {
            Messages = new HashSet<Message>();
            Users = new HashSet<ApplicationUser>();


        }
    }
}