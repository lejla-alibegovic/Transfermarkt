using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class MatchDetails
    {
        public int MatchId { get; set; }
        public string HomeClub { get; set; }
        public string AwayClub { get; set; }
        public string StadiumName { get; set; }
        public string LeagueName { get; set; }
        public string HomeClubLogo { get; set; }
        public string AwayClubLogo { get; set; }
        public List<CornersMatch> CornersMatches { get; set; }
        public List<FoulsMatch> FoulsMatch { get; set; }
        public List<GoalsMatch> GoalsMatch { get; set; }
        public int GoalsScored { get; set; }
        public DateTime TimePlayed { get; set; }
    }
}
