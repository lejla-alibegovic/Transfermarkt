using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transfermarkt.Web.Reports
{
    public class Report1VM
    {
        public DateTime TimePlayed { get; set; }
        public string Stadium { get; set; }
        public string HomeClub { get; set; }
        public string AwayClub { get; set; }
        public string League { get; set; }

        public static List<Report1VM> Get()
        {
            return new List<Report1VM> { };
        }
    }
}