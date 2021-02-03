using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transfermarkt.Web.Models
{
    public class Contract : IEntity
    {
        public int Id { get; set ; }

        [Required]
        public DateTime SignedDate { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }

        [ForeignKey(nameof(Club))]
        public int ClubId { get; set; }
        public Club Club { get; set; }

        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
