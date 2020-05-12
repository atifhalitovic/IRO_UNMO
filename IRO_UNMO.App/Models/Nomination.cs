using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class Nomination
    {
        [Key]
        public int NominationId { get; set; }
        public Applicant Applicant { get; set; }
        public string ApplicantId { get; set; }
        public int UniversityId { get; set; }
        public University University { get; set; }
        public string LearningAgreement { get; set; }
        public string WorkPlan { get; set; }
        public string CV { get; set; }
        public string EngProficiency { get; set; }
        public string TranscriptOfRecords { get; set; }
        public string MotivationLetter { get; set; }
        public string ReferenceLetter { get; set; }
        public bool Verified { get; set; }
        public string StatusOfNomination { get; set; }

        public Comment Comments { get; set; }
        public int? CommentsId { get; set; }

        public DateTime CreatedNom { get; set; }
        public DateTime LastEdited { get; set; }
    }
}
