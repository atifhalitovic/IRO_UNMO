using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.WebAPI.Database
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UniqueCode { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public DateTime LastLogin { get; set; }
        public string SignalRToken { get; set; }
    }
}
