using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class JSChartData
    {
        public List<int> data { get; set; }
        public List<string> label { get; set; }
        public JSChartData()
        {
            data = new List<int>();
            label = new List<string>();
        }
    }
}