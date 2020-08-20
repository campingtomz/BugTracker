using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        #region Parents/Children
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<ProjectHistory> Histories { get; set; }
        #endregion

        #region Actual Properties
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsArchive { get; set; }

        #endregion

        #region Constructor
        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
            Histories = new HashSet<ProjectHistory>();

        }
        #endregion

    }
}