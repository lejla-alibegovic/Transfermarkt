using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class HistoryPlayerVM
    {
        public string ClubName { get; set; }
        public DateTime SignedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
