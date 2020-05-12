using IRO_UNMO.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.ViewModels
{
    public class ApplicantsVM
    {
        public List<Applicant> IApplicants { get; set; }
        public List<Applicant> OApplicants { get; set; }
    }
}
