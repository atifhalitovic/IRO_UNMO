﻿using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.ViewModels
{
    public class CreateNewNomVM
    {
        public string ApplicantId { get; set; }
        public Applicant Applicant { get; set; }
        public DateTime LastEdited { get; set; }
        public int OfferId { get; set; }
        public Offer Offer { get; set; }
        public List<Offer> Offers { get; set; }
        public List<SelectListItem> Universities { get; set; }
    }
}
