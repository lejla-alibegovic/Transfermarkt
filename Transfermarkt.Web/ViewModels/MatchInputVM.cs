using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Transfermarkt.Web.Models;

namespace Transfermarkt.Web.ViewModels
{
    public class MatchInputVM
    {
        public int Id { get; set; }

        [Required]
        public DateTime TimePlayed { get; set; }
        public int LeagueId { get; set; }
        public List<SelectListItem> Leagues { get; set; }
        public int StadiumId { get; set; }
        public List<SelectListItem> Stadiums { get; set; }
        public int HomeClubId { get; set; }
        public List<SelectListItem> HomeClubs { get; set; }
        public int AwayClubId { get; set; }
        public List<SelectListItem> AwayClubs { get; set; }
    }
}
