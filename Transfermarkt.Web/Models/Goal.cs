using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfermarkt.Web.Models.Enums;

namespace Transfermarkt.Web.Models
{
    public class Goal : IEntity
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Minute of the goal can only have numbers")]
        public int Minute { get; set; }
        [Required]
        public HalfTime Time { get; set; }

        [ForeignKey(nameof(Assistant))]
        public int? AssistantId { get; set; }
        public Player Assistant { get; set; }

        [ForeignKey(nameof(Scorer))]
        public int ScorerId { get; set; }
        public Player Scorer { get; set; }

        [ForeignKey(nameof(Match))]
        public int MatchId { get; set; }
        public Match Match { get; set; }
    }
}
