using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.Models
{
    public class RefereeMatch : IEntity
    {
        public int Id { get ; set ; }

        [ForeignKey(nameof(Match))]
        public int MatchId { get; set; }
        public Match Match { get; set; }

        [ForeignKey(nameof(Referee))]
        public int RefereeId { get; set; }
        public Referee Referee { get; set; }
    }
}
