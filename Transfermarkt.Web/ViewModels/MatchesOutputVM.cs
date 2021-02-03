using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class MatchesOutputVM
    {
        public int MatchId { get; set; }
        public DateTime TimePlayed { get; set; }
        public string Stadium { get; set; }
        public string HomeClub { get; set; }
        public string AwayClub { get; set; }
        public string League { get; set; }
        public int LeagueId { get; set; }
    }
}
