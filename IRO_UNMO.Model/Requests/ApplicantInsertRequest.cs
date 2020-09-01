using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IRO_UNMO.Model.Requests
{
    public class ApplicantInsertRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string UniqueCode { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Verified { get; set; }
        public string StudyCycle { get; set; }
        public string StudyField { get; set; }
        public int CountryId { get; set; }
        public int UniversityId { get; set; }
        public string TypeOfApplication { get; set; }
        public string FacultyName { get; set; }
    }
}
