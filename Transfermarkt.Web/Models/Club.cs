using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transfermarkt.Web.Models
{
    public class Club : IEntity
    {
        public int Id { get ; set ; }
        [Required]
        [StringLength(30)]
        [RegularExpression("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$", ErrorMessage = "Name of the club have to start with capital letter")]
        public string Name { get; set; }
        [Required]
        [StringLength(3)]
        [RegularExpression("[A-Z]{3,}", ErrorMessage = "Abbreviation can only have capital letters")]
        public string Abbreviation { get; set; }
        [Required]
        [StringLength(30)]
        [RegularExpression("[A-Z]{1}[a-z]{2,}", ErrorMessage = "Nickname have to start with the capital letter")]
        public string Nickname { get; set; }
        [Required]
        public DateTime Founded { get; set; }
        [Required]
        public string Logo { get; set; }
        [Required]
        [RegularExpression("[0-9]{3,}", ErrorMessage = "Market values can only have numbers")]
        public int MarketValue { get; set; }
        [ForeignKey(nameof(City))]
        public int CityId { get; set; }
        public City City { get; set; }

        [ForeignKey(nameof(League))]
        public int LeagueId { get; set; }
        public League League { get; set; }
    }
}
