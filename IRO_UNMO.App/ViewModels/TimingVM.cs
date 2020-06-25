using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.ViewModels
{
    public class TimingVM
    {
        public Timing current { get; set; }
        public List<SelectListItem> Semesters { get; set; }
    }
}
