using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Areas.Admin.ViewModels
{
    public class EditPartnersVMa
    {
        public int UniversityId { get; set; }
        public University University { get; set; }
        public List<SelectListItem> Countries { get; set; }
    }
}
