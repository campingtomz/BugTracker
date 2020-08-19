using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class ProjectHistory : History
    {
        public int Id { get; set; }
      
        public int ProjectId { get; set; }
        public virtual Project project { get; set; }


    }
}