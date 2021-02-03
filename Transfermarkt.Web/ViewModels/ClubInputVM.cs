using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Transfermarkt.Web.ViewModels
{
    public class ClubInputVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$", ErrorMessage = "Name of the club have to start with capital letter")]
        public string Name { get; set; }

        [Required]
        [StringLength(3)]
        [RegularExpression("[A-Z]{3,}", ErrorMessage = "Abbreviation can only have capital letters")]
        public string Abbreviation { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression("[A-Z]{1}[a-z]{2,}", ErrorMessage = "Nickname have to start with the capital letter")]
        public string Nickname { get; set; }

        [Required]
        public DateTime Founded { get; set; }
        [Required]
        public IFormFile Logo { get; set; }
        
        public string LogoPath { get; set; }

        [Required]
        [RegularExpression("[0-9]{3,}", ErrorMessage = "Market values can only have numbers")]
        public int MarketValue { get; set; }
        public int CityId { get; set; }
        public List<SelectListItem> Cities { get; set; }

        public int LeagueId { get; set; }
        public List<SelectListItem> Leagues { get; set; }

    }
}
