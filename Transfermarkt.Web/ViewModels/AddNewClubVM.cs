using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class AddNewClubVM
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Nickname { get; set; }    
        public DateTime Founded { get; set; }
        public string Logo { get; set; }
        public int MarketValue { get; set; }
        public List<SelectListItem> Citys { get; set; }
        public List<SelectListItem> League { get; set; }
    }
}
