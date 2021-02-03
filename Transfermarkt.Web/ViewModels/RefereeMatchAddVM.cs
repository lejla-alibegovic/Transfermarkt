using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transfermarkt.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Transfermarkt.Web.ViewModels
{
    public class RefereeMatchAddVM
    {
        public List<SelectListItem> MatchList { get; set; }
        public List<SelectListItem> RefereeList { get; set; }
    }
}
