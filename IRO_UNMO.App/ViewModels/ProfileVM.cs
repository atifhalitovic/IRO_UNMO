using IRO_UNMO.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.ViewModels
{
    public class ProfileVM
    {
        public Applicant Applicant { get; set; }
        public Application Application { get; set; }
        public List<Nomination> Nominations { get; set; }
    }
}
