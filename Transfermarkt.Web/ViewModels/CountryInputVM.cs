using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Transfermarkt.Web.Models.Enums;

namespace Transfermarkt.Web.ViewModels
{
    public class CountryInputVM
    {
        [Required]
        [StringLength(30)]
        [RegularExpression("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$", ErrorMessage = "Name of the country have to start with capital letter")]
        public string Name { get; set; }

        [Required]
        [StringLength(4)]
        public string Code { get; set; }
        public string Confederation { get; set; }
        public IEnumerable<SelectListItem> Confederations { get; set; }
    }
}
