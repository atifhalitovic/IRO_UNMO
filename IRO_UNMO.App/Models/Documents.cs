using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class Documents
    {
        [Key]
        public int DocumentsId { get; set; }
        public string LearningAgreement { get; set; }
        public string CV { get; set; }
        public string Passport { get; set; }
        public string EngProficiency { get; set; }
        public string TranscriptOfRecords { get; set; }
        public string MotivationLetter { get; set; }
        public string ReferenceLetter { get; set; }

        //Issued by UNMO, just download - no upload (upload by UNMO coordinator)
        public string LearningAgreementHost { get; set; }
        public string CertificateOfArrival { get; set; }
        public string CertificateOfDeparture { get; set; }
        public string StudentTranscriptOfRecords { get; set; }
        public string StudentRecordSheet { get; set; }
    }
}
