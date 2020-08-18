using IRO_UNMO.App.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class Applicant
    {
        [ForeignKey("ApplicationUser")]
        public string ApplicantId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        //[Required(ErrorMessage = "Please enter which type of applicant are you.")]
        public string TypeOfApplication { get; set; }
        //[Required(ErrorMessage = "Please enter the name of your faculty.")]
        public string FacultyName { get; set; }
        //[Required(ErrorMessage = "Please enter the name of study field.")]
        public string StudyField { get; set; }
        //[Required(ErrorMessage = "Please enter the name of study cycle.")]
        public string StudyCycle { get; set; }
        //[Required(ErrorMessage = "Please select your university.")]
        public int UniversityId { get; set; }
        public University University { get; set; }
        public DateTime CreatedProfile { get; set; }
        public DateTime LastEdited { get; set; }
        public bool Verified { get; set; }
    }
}
