using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class Contact
    {
        public int ContactId { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string StreetName { get; set; }
        public string PlaceName { get; set; }
        public string PostalCode { get; set; }
        public Country Country { get; set; }
        public int? CountryId { get; set; }
    }
}
