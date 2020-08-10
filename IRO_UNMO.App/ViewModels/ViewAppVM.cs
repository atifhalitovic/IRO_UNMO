using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.ViewModels
{
    public class ViewAppVM
    {
        public Applicant Applicant { get; set; }
        public Application Application { get; set; }

        public List<Comment> Comments { get; set; }
        public string NewComment { get; set; }

        public string StatusOfApplication { get; set; }
        public List<SelectListItem> Statuses { get; set; }

        //Issued by UNMO, just download - no upload (upload by UNMO coordinator)
        //for admin upload
        public IFormFile LearningAgreementHost { get; set; }
        public IFormFile StudentRecordSheet { get; set; }
        public IFormFile CertificateOfArrival { get; set; }
        public IFormFile CertificateOfDeparture { get; set; }
        public IFormFile StudentTranscriptOfRecords { get; set; }
    }
}
