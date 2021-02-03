using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfermarkt.Web.Models.Enums;

namespace Transfermarkt.Web.Models
{
    public class Player : IEntity
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

        [Required]
        [RegularExpression("[0-9]{1,2}")]
        public int Jersey { get; set; }

        [Required]
        [RegularExpression("[0-9]{3}",ErrorMessage ="Your height can have 3 digits only")]
        public int Height { get; set; }

        [Required]
        [RegularExpression("[0-9]{2,3}",ErrorMessage ="Your weight can have 2 or 3 digits only")]
        public int Weight { get; set; }

        [Required]
        [RegularExpression("[0-9]{2,}", ErrorMessage = "Your value can have 2 or more digits")]
        public int Value { get; set; }

        [Required]
        public StrongerFoot StrongerFoot { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        [ForeignKey(nameof(Birthplace))]
        [Required]
        public int BirthplaceId { get; set; }
        public City Birthplace { get; set; }
    }
}
