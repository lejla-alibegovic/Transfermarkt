using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transfermarkt.Web.Models
{
    public class Foul : IEntity
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Minute of the foul can only have numbers")]
        public int Minute { get; set; }
        public bool Penalty { get; set; }

        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        [ForeignKey(nameof(Victim))]
        public int VictimId { get; set; }
        public Player Victim { get; set; }

        [ForeignKey(nameof(Match))]
        public int MatchId { get; set; }
        public Match Match { get; set; }
    }
}
