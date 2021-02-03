using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class HistoryOfPlayerVM
    {
        public string FullName { get; set; }
        public List<HistoryPlayerVM> list { get; set; }
    }
}
