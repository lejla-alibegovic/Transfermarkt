using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Transfermarkt.Web.Models
{
    public class Position : IEntity
    {
        public int Id { get ; set ; }
        [Required]
        [StringLength(30)]
        [RegularExpression("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$", ErrorMessage = "Name of the position can only have letters")]
        public string Name { get; set; }
        [Required]
        [StringLength(4)]
        [RegularExpression("[A-Z]{1,4}", ErrorMessage = "Abbreviation of the position can only have letters")]
        public string Abbreviation { get; set; }
    }
}
