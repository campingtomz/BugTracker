using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class ProjectHistory : History
    {
        public ApplicationUser User { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

    }
}