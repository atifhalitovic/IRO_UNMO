using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.ViewModels
{
    public class OffersVM
    {
        public List<Offer> Offers { get; set; }
        public List<Offer> EOffers { get; set; }
        public List<Offer> UOffers { get; set; }
        public Offer Offer { get; set; }
    }
}
