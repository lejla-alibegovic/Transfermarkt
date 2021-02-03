using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class RefereeInputVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$", ErrorMessage = "First name of the player have to start with capital letter")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$", ErrorMessage = "Middle name of the player have to start with capital letter")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$", ErrorMessage = "Last name of the player have to start with capital letter")]
        public string LastName { get; set; }
        public int CountryId { get; set; }
        public List<SelectListItem> Countries { get; set; }
    }
}
