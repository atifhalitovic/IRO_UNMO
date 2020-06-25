using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Areas.Admin.ViewModels
{
    public class EditOffersVM
    {
        public int OfferId { get; set; }
        public Offer Offer { get; set; }
        public List<SelectListItem> Universities { get; set; }
        public List<SelectListItem> Semesters { get; set; }
        public List<SelectListItem> Cycles { get; set; }
        public List<SelectListItem> Programmes { get; set; }
    }
}
