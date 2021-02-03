using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class GoalsMatch
    {
        public int MatchId { get; set; }
        public int Minute { get; set; }
        public string ScorerName { get; set; }
        public int ScorerId { get; set; }
    }
}
