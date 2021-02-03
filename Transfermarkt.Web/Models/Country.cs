using System.ComponentModel.DataAnnotations;
using Transfermarkt.Web.Models.Enums;

namespace Transfermarkt.Web.Models
{
    public class Country : IEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$", ErrorMessage = "Name of the country have to start with capital letter")]
        public string Name { get; set; }

        [Required]
        [StringLength(4)]
        public string Code { get; set; }
        public Confederations Confederation { get; set; }
    }
}
