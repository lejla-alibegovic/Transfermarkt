using System.ComponentModel.DataAnnotations.Schema;

namespace Transfermarkt.Web.Models
{
    public class Favorite : IEntity
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(Club))]
        public int ClubId { get; set; }
        public Club Club { get; set; }
    }
}
