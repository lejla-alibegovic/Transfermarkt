using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Transfermarkt.Web.ViewModels
{
    public class PlayerPositionsInputVM
    {
        public int PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PositionId { get; set; }
        public List<int> Ids { get; set; }
        public List<SelectListItem> Positions { get; set; }
    }
}
