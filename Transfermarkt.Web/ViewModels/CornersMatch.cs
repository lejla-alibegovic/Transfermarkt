using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class CornersMatch
    {
        public int MatchId { get; set; }
        public string TakerName { get; set; }
        public int Minute { get; set; }
        public int TakerId { get; set; }
    }
}
