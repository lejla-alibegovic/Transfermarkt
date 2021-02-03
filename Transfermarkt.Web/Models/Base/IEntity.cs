using System.ComponentModel.DataAnnotations;

namespace Transfermarkt.Web.Models
{
    public interface IEntity
    { 
        [Key]
        int Id { get; set; }
    }
}
