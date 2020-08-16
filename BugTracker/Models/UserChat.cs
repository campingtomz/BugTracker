using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class UserChat
    {

        public int Id { get; set; }
        #region Parents/Children
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<ChatMessage> Messages { get; set; }
        #endregion
        #region Actual Properties
       
        public List<string> UsersId { get; set; }

        public bool isArchived { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }


        #endregion
        public UserChat()
        {
            Messages = new HashSet<ChatMessage>();
            Users = new HashSet<ApplicationUser>();
            UsersId = new List<string>();
        }
    }
}