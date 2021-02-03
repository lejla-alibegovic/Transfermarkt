using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class CoachClubInputVM
    {
  
        public int CoachId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ClubId { get; set; }
        public DateTime ContractSigned { get; set; }
        public DateTime ContractExpired { get; set; }
      
        public List<int> Ids { get; set; }
        public List<SelectListItem> Clubs { get; set; }

        
    }
}
