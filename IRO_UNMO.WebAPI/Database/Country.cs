using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.WebAPI.Database
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
