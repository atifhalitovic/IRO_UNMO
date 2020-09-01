using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.WebAPI.Database
{
    public class Applicant
    {
        [ForeignKey("ApplicationUser")]
        public string ApplicantId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string TypeOfApplication { get; set; }
        public string FacultyName { get; set; }
        public string StudyField { get; set; }
        public string StudyCycle { get; set; }
        public int UniversityId { get; set; }
        public University University { get; set; }
        public DateTime CreatedProfile { get; set; }
        public DateTime LastEdited { get; set; }
        public bool Verified { get; set; }
    }
}
