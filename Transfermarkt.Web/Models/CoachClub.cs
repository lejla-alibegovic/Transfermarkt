using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transfermarkt.Web.Models
{
    public class CoachClub : IEntity
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Coach))]
        public int CoachId { get; set; }
        public Coach Coach { get; set; }

        [ForeignKey(nameof(Club))]
        public int ClubId { get; set; }
        public Club Club { get; set; }
        [Required]
        public DateTime ContractSigned { get; set; }
        [Required]
        public DateTime ContractExpired { get; set; }
    }
}
