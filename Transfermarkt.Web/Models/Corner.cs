using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using Transfermarkt.Web.Models.Enums;

namespace Transfermarkt.Web.Models
{
    public class Corner : IEntity
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Minute of the corner can only have numbers")]
        public int MinuteAwarded { get; set; }
        [Required]
        public HalfTime HalfTime { get; set; }

        [ForeignKey(nameof(Taker))]
        public int TakerId { get; set; }
        public Player Taker { get; set; }

        [ForeignKey(nameof(Match))]
        public int MatchId { get; set; }
        public Match Match { get; set; }
        
    }
}
