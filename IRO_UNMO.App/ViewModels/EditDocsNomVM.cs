using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.ViewModels
{
    public class EditDocsNomVM
    {
        public Nomination Nomination { get; set; }
        public int NominationId { get; set; }
        public Applicant Applicant { get; set; }
        public string ApplicantId { get; set; }

        public IFormFile LearningAgreement { get; set; }
        public IFormFile WorkPlan { get; set; }
        public IFormFile CV { get; set; }
        public IFormFile Passport { get; set; }
        public IFormFile EngProficiency { get; set; }
        public IFormFile TranscriptOfRecords { get; set; }
        public IFormFile MotivationLetter { get; set; }
        public IFormFile ReferenceLetter { get; set; }


        //Issued by UNMO, just download - no upload (upload by UNMO coordinator)
        public IFormFile CertificateOfArrival { get; set; }
        public IFormFile CertificateOfDeparture { get; set; }
        public IFormFile StudentTranscriptOfRecords { get; set; }
        public IFormFile StudentRecordSheet { get; set; }
    }
}
