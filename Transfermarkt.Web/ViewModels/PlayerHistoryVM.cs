using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class PlayerHistoryVM
    {
        public List<SelectListItem> players { get; set; }
    }
}
