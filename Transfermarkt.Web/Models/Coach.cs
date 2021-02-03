using System;
using System.ComponentModel.DataAnnotations;

namespace Transfermarkt.Web.Models
{
    public class Coach : IEntity
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        [RegularExpression("[A-Z][a-z]*", ErrorMessage = "First name can only have letters and must start with a capital letter.")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(30)]
        [RegularExpression("[A-Z][a-z]*", ErrorMessage = "Middle name can only have letters and must start with a capital letter.")]
        public string MiddleName { get; set; }
        [Required]
        [StringLength(30)]
        [RegularExpression("[A-Z][a-z]*", ErrorMessage = "Last name can only have letters and must start with a capital letter.")]
        public string LastName { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }
    }
}
