using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.ViewModels
{
    public class ViewAppVM
    {
        public Applicant Applicant { get; set; }
        public Application Application { get; set; }

        public string StatusOfApplication { get; set; }
        public List<SelectListItem> Statuses { get; set; }
    }
}
