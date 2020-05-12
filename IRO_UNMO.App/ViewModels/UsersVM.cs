using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.ViewModels
{
    public class UsersVM
    {
        //Podaci od svakog korisnika
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UniqueCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Verified { get; set; }
        public string StudyCycle { get; set; }
        public List<SelectListItem> StudyCycles { get; set; }
        public string StudyField { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public int CountryId { get; set; }
        public List<SelectListItem> Universities { get; set; }
        public int UniversityId { get; set; }

        public string TypeOfApplication { get; set; }
        public List<SelectListItem> TypesOfApplicant { get; set; }

        public string FacultyName { get; set; }
        public List<SelectListItem> Faculties { get; set; }

        //Administrator details

        // CURRENTLY NO DETAILS

        //Applicant details
    }
}
