using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Transfermarkt.Web.Models.Enums;

namespace Transfermarkt.Web.ViewModels
{
    public class FoulInputVM
    {
        [Required]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Minute of the foul can only have numbers")]
        public int Minute { get; set; }
        public bool Penalty { get; set; }
        public int PlayerId { get; set; }
        public int VictimId { get; set; }
        public int MatchId { get; set; }
        public List<SelectListItem> Players { get; set; }
        public List<SelectListItem> Victims { get; set; }
        public int LeagueId { get; set; }
    }
}
