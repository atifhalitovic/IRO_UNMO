using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.ViewModels
{
    public class EditAppVM
    {
        public Application Application { get; set; }
        public int ApplicationId { get; set; }
        public int? InfoId { get; set; }

        //Info
        public Info Info { get; set; }
        public string Gender { get; set; }
        public int NationalityId { get; set; }
        public int CitizenshipId { get; set; }
        public int? SecondCitizenshipId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string PassportNumber { get; set; }
        public string PassportIssueDate { get; set; }
        public string PassportExpiryDate { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string MothersMaidenName { get; set; }

        //Contacts
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string StreetName { get; set; }
        public string PlaceName { get; set; }
        public string PostalCode { get; set; }
        public int CountryId { get; set; }

        //HomeInstitution
        public string OfficialName { get; set; }
        public string DepartmentName { get; set; }
        public string LevelOfEducation { get; set; }
        public string CurrentTermOrYear { get; set; }
        public string StudyProgramme { get; set; }
        public string CoordinatorFullName { get; set; }
        public string CoordinatorPosition { get; set; }
        public string CoordinatorEmail { get; set; }
        public string CoordinatorAddress { get; set; }
        public string CoordinatorPhoneNum { get; set; }

        //Languages
        public string Native { get; set; }
        public string ForeignFirst { get; set; }
        public string ForeignFirstProficiency { get; set; }
        public string ForeignSecond { get; set; }
        public string ForeignSecondProficiency { get; set; }
        public string ForeignThird { get; set; }
        public string ForeignThirdProficiency { get; set; }
        public int ForeignExperienceNumber { get; set; }

        //Other
        public string MedicalInfo { get; set; }
        public string AdditionalRequests { get; set; }

        public List<SelectListItem> Genders { get; set; }
        public List<SelectListItem> Languages { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public List<SelectListItem> StudyCycles { get; set; }
        public List<SelectListItem> StudyTerms { get; set; }
        public List<SelectListItem> StudyProgrammes { get; set; }
        public List<SelectListItem> LangProfiency { get; set; }
    }
}
