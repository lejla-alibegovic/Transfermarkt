using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class LeagueInputVM
    {
        [Required]
        [StringLength(30)]
        [RegularExpression("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$", ErrorMessage = "Name of the league have to start with capital letter")]
        public string Name { get; set; }
        public string NameCountry { get; set; }
        public int CountryId { get; set; }
    }
}
