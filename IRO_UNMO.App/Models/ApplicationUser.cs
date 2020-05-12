using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UniqueCode { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
