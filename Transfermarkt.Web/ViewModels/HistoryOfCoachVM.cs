using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class HistoryOfCoachVM
    {
        public string FullName { get; set; }
        public List<CoachHistory> list { get; set; }
    }
}
