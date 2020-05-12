using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.ViewModels
{
    public class ViewNomVM
    {
        public Applicant Applicant { get; set; }
        public Nomination Nomination { get; set; }
        public List<Comment> Comments { get; set; }
        public string NewComment { get; set; }

        public string StatusOfNomination { get; set; }
        public List<SelectListItem> Statuses { get; set; }

        public IFormFile LearningAgreement { get; set; }
        public IFormFile WorkPlan { get; set; }
        public IFormFile CV { get; set; }
        public IFormFile Passport { get; set; }
        public IFormFile EngProficiency { get; set; }
        public IFormFile TranscriptOfRecords { get; set; }
        public IFormFile MotivationLetter { get; set; }
        public IFormFile ReferenceLetter { get; set; }
    }
}
