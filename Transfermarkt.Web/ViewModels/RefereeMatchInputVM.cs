using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class RefereeMatchInputVM
    {
        public int RefereeId { get; set; }
        public DateTime TimePlayed { get; set; }
        public int MatchId { get; set; }
        public List<int> Ids { get; set; }
        public List<SelectListItem> Referees { get; set; } 
    }
}
