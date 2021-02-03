using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transfermarkt.Web.Models
{
    public class Referee : IEntity
    {
        public int Id { get ; set ; }
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

        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
