using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class ClubOutputVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Nickname { get; set; }
        public DateTime Founded { get; set; }
        public IFormFile Logo { get; set; }
        public int MarketValue { get; set; }
        public int CityId { get; set; }
        public List<SelectListItem> Cities { get; set; }
    }
}
