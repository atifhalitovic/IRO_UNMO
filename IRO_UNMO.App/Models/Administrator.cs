using IRO_UNMO.App.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class Administrator
    {
        [ForeignKey("ApplicationUser")]
        public string AdministratorId { get; set; }
        public DateTime CreatedProfile { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
