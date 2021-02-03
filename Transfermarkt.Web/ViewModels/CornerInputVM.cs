using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Transfermarkt.Web.Models.Enums;

namespace Transfermarkt.Web.ViewModels
{
    public class CornerInputVM
    {
        [Required]
        [RegularExpression("[0-9]{1,3}", ErrorMessage = "Minute of the corner can only have numbers")]
        public int MinuteAwarded { get; set; }

        [Required]
        public HalfTime HalfTime { get; set; }
        public IEnumerable<SelectListItem> HalfTimes { get; set; }
        public int TakerId { get; set; }
        public int MatchId { get; set; }
        public List<SelectListItem> Players { get; set; }
        public int LeagueId { get; set; }
    }
}
