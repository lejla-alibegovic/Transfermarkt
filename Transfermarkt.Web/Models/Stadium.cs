using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transfermarkt.Web.Models
{
    public class Stadium : IEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$", ErrorMessage = "Name of the stadium have to start with capital letter")]
        public string Name { get; set; }

        [Required]
        public DateTime DateBuilt { get; set; }

        [Required]
        [RegularExpression("^\\d+$", ErrorMessage="Capacity can only have numeric values")]
        public int Capacity { get; set; }

        [ForeignKey(nameof(Club))]
        public int ClubId { get; set; }
        public Club Club { get; set; }
    }
}
