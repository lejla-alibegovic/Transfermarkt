using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class SearchMatchVM
    {
        public string HomeClub { get; set; }
        public string AwayClub { get; set; }
        public string RefereeName { get; set; }
        public string StadiumName { get; set; }
        public string League { get; set; }
    }
}
