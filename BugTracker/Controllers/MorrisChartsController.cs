using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.ViewModels;

namespace BugTracker.Controllers
{
    public class MorrisChartsController : Controller
    {//{ priority: 'Immed', value: 20 },
                //{ priority: 'High', value: 10 },
                //{ priority: 'Medium', value: 5 },
                //{ priority: 'Low', value: 5 },
                //{ priority: 'None', value: 20 }
        // GET: MorrisCharts  
        private ApplicationDbContext db = new ApplicationDbContext();

    public JsonResult GetTicketPriorityData()
        {
            var data = new List<MorrisChartData>();
            var tickets = db.Tickets.ToList();
            foreach (var priority in db.TicketPriorities.ToList())
            {
                data.Add(new MorrisChartData()
                {
                Label = priority.Name,
                Value = tickets.Where(t => t.TicketPriorityId == priority.Id).Count()
                });
            }
            return Json(data);
        }
    }
}