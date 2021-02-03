using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class ClubDetailsVM
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Nickname { get; set; }
        public DateTime Founded { get; set; }
        public string Logo { get; set; }
        public int MarketValue { get; set; }
        public string city { get; set; }
        public string League { get; set; }
    }
}
