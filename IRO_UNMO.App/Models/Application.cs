using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public Applicant Applicant { get; set; }
        public string ApplicantId { get; set; }
        public Info Infos { get; set; }
        public int? InfoId { get; set; }
        public Contact Contacts { get; set; }
        public int? ContactId { get; set; }
        public HomeInstitution HomeInstitutions { get; set; }
        public int? HomeInstitutionId { get; set; }
        public Language Languages { get; set; }
        public int? LanguageId { get; set; }
        public Other Others { get; set; }
        public int? OtherId { get; set; }
        public Documents Documents { get; set; }
        public int? DocumentsId { get; set; }

        public Timing Timing { get; set; }
        public int? TimingId { get; set; }

        public string StatusOfApplication { get; set; }
        public DateTime CreatedApp { get; set; }
        public DateTime LastEdited { get; set; }
        public bool Finished { get; set; }
        public DateTime FinishedTime { get; set; }
    }
}
