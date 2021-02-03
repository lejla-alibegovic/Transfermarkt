using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class FoulsMatch
    {
        public int Minute { get; set; }
        public string TakerName { get; set; }
        public int MatchId { get; set; }
        public bool Penalty { get; set; }
        public int TakerId { get; set; }
    }
}
