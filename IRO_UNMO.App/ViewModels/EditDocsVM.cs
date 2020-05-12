using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.ViewModels
{
    public class EditDocsVM
    {
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public int? DocumentsId { get; set; }
        public Documents Documents { get; set; }

        public IFormFile LearningAgreement { get; set; }
        public IFormFile WorkPlan { get; set; }
        public IFormFile CV { get; set; }
        public IFormFile Passport { get; set; }
        public IFormFile EngProficiency { get; set; }
        public IFormFile TranscriptOfRecords { get; set; }
        public IFormFile MotivationLetter { get; set; }
        public IFormFile ReferenceLetter { get; set; }


        //Issued by UNMO, just download - no upload (upload by UNMO coordinator)
        //for admin upload
        public IFormFile LearningAgreementHost { get; set; }
        public IFormFile StudentRecordSheet { get; set; }
        public IFormFile CertificateOfArrival { get; set; }
        public IFormFile CertificateOfDeparture { get; set; }
        public IFormFile StudentTranscriptOfRecords { get; set; }
    }
}
