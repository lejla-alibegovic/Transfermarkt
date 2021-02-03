using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfermarkt.Web.ViewModels
{
    public class PlayerCriteriaVM
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Jersey { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Value { get; set; }
        public string StrongerFoot { get; set; }
        public DateTime Birthdate { get; set; }
        public string Birthplace { get; set; }
    }
}
