using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class CoachHistory
    {
        public string name { get; set; }
        public string league { get; set; }
        public DateTime ContractSigned { get; set; }
        public DateTime ContractExpired { get; set; }
    }
}
