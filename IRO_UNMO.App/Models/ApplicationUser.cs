using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Name is mandatory field.")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[A-Ža-ž\s]+$")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is mandatory field.")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[A-Ža-ž\s]+$")]
        public string Surname { get; set; }
        public string UniqueCode { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public DateTime LastLogin { get; set; }
        public string SignalRToken { get; set; }
    }
}
