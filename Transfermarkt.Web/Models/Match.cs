using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transfermarkt.Web.Models
{
    public class Match : IEntity
    {
        public int Id { get ; set; }
        [Required]
        public DateTime TimePlayed { get; set; }

        [ForeignKey(nameof(League))]
        public int LeagueId { get; set; }
        public League League { get; set; }

        [ForeignKey(nameof(Stadium))]
        public int StadiumId { get; set; }
        public Stadium Stadium { get; set; }

        [ForeignKey(nameof(HomeClub))]
        public int HomeClubId { get; set; }
        public Club HomeClub { get; set; }

        [ForeignKey(nameof(AwayClub))]
        public int AwayClubId { get; set; }
        public Club AwayClub { get; set; }
    }
}
