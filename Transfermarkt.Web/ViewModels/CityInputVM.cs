using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Transfermarkt.Web.ViewModels
{
    public class CityInputVM
    {
        [Required]
        [StringLength(30)]
        [RegularExpression("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$", ErrorMessage = "Name of the city have to start with capital letter")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Your postal code can have only digits")]
        public int PostalCode { get; set; }
        public int CountryId { get; set; }
        public List<SelectListItem> Countries { get; set; }
    }
}
