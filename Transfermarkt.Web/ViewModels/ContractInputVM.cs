using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Transfermarkt.Web.Models.Enums;

namespace Transfermarkt.Web.ViewModels
{
    public class ContractInputVM
    {
        [Required]
        public DateTime SignedDate { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }
        public int ClubId { get; set; }
        public int PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<int> Ids { get; set; }
        public List<SelectListItem> Clubs { get; set; }

    }
}
