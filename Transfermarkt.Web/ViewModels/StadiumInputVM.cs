using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class StadiumInputVM
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        [RegularExpression("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$", ErrorMessage = "Name of the stadium have to start with capital letter")]
        public string Name { get; set; }
        public int LeagueId { get; set; }
        [Required]
        public DateTime DateBuilt { get; set; }
        [Required]
        [RegularExpression("^\\d+$", ErrorMessage = "Capacity can only have numeric values")]
        public int Capacity { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        //public List<SelectListItem> Clubs { get; set; }
    }
}
