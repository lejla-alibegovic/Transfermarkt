using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Transfermarkt.Web.Models
{
    public class User : IdentityUser<int>, IEntity
    {
        
        public string FirstName { get; set; }
       
        public string MiddleName { get; set; }
       
        public string LastName { get; set; }
    }
}
