using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Transfermarkt.Web.Models.Enums;

namespace Transfermarkt.Web.ViewModels
{
    public class GoalInputVM
    {
        [Required]
        [RegularExpression("[0-9]{1,3}", ErrorMessage = "Minute of the goal can only have numbers")]
        public int Minute { get; set; }

        [Required]
        public HalfTime Time { get; set; }
        public IEnumerable<SelectListItem> Times { get; set; }
        public int? AssistantId { get; set; }
        public int ScorerId { get; set; }
        public int MatchId { get; set; }
        public List<SelectListItem> ScoredPlayers { get; set; }
        public List<SelectListItem> AssistedPlayers { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int LeagueId { get; set; }
    }
}
