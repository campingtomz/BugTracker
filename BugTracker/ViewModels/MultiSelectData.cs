using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class MultiSelectData
    {
       public List<Data> UserInProjectByRole { get; set; }
        public List<Data> UserNotInProjectByRole { get; set; }

        public int[] UserInProjectByRoleIds { get; set; }

    }
    public class Data
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
}