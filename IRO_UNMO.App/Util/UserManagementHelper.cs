using IRO_UNMO.App.Data;
using IRO_UNMO.App.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRO_UNMO.Util
{
    public class UserManagementHelper
    {
        private ApplicationDbContext _db;

        public UserManagementHelper(ApplicationDbContext db)
        {
            _db = db;
        }

        public UsersVM prepUser()
        {

            UsersVM model = new UsersVM();

            model.Countries = _db.Country.Select(x => new SelectListItem()
            {
                Value = x.CountryId.ToString(),
                Text = x.Name
            }).ToList();

            model.Universities = _db.University.Select(x => new SelectListItem()
            {
                Value = x.UniversityId.ToString(),
                Text = x.Name
            }).ToList();

            model.TypesOfApplicant = new List<SelectListItem>();

            model.TypesOfApplicant.Add(new SelectListItem()
            {
                Value = "Student",
                Text = "Student"
            });

            model.TypesOfApplicant.Add(new SelectListItem()
            {
                Value = "Staff",
                Text = "Staff"
            });

            model.StudyCycles = new List<SelectListItem>();

            model.StudyCycles.Add(new SelectListItem()
            {
                Value = "Bachelor",
                Text = "Bachelor"
            });

            model.StudyCycles.Add(new SelectListItem()
            {
                Value = "Master",
                Text = "Master"
            });

            model.Faculties = new List<SelectListItem>();

            model.Faculties.Add(new SelectListItem()
            {
                Value = "Agromediteranski fakultet",
                Text = "Agromediteranski fakultet"
            });

            model.Faculties.Add(new SelectListItem()
            {
                Value = "Ekonomski fakultet",
                Text = "Ekonomski fakultet"
            });

            model.Faculties.Add(new SelectListItem()
            {
                Value = "Fakultet humanističkih nauka",
                Text = "Fakultet humanističkih nauka"
            });

            model.Faculties.Add(new SelectListItem()
            {
                Value = "Fakultet informacijskih tehnologija",
                Text = "Fakultet informacijskih tehnologija"
            });

            model.Faculties.Add(new SelectListItem()
            {
                Value = "Građevinski fakultet",
                Text = "Građevinski fakultet"
            });

            model.Faculties.Add(new SelectListItem()
            {
                Value = "Mašinski fakultet",
                Text = "Mašinski fakultet"
            });

            model.Faculties.Add(new SelectListItem()
            {
                Value = "Nastavnički fakultet",
                Text = "Nastavnički fakultet"
            });

            model.Faculties.Add(new SelectListItem()
            {
                Value = "Pravni fakultet",
                Text = "Pravni fakultet"
            });


            return model;
        }

        public EditAppVM prepApp()
        {
            EditAppVM model = new EditAppVM();

            model.Languages = new List<SelectListItem>();
            model.Languages.Add(new SelectListItem()
            {
                Value = "",
                Text = "Please select your language"
            });

            model.Languages.Add(new SelectListItem()
            {
                Value = "bosnian",
                Text = "bosnian"
            });

            model.Languages.Add(new SelectListItem()
            {
                Value = "croatian",
                Text = "croatian"
            });

            model.Languages.Add(new SelectListItem()
            {
                Value = "serbian",
                Text = "serbian"
            });

            model.Languages.Add(new SelectListItem()
            {
                Value = "spanish",
                Text = "spanish"
            });

            model.Genders = new List<SelectListItem>();

            model.Genders.Add(new SelectListItem()
            {
                Value = "",
                Text = "Please select your gender"
            });

            model.Genders.Add(new SelectListItem()
            {
                Value = "Male",
                Text = "Male"
            });

            model.Genders.Add(new SelectListItem()
            {
                Value = "Female",
                Text = "Female"
            });

            model.Genders.Add(new SelectListItem()
            {
                Value = "Not specified",
                Text = "Not specified"
            });

            model.Countries = _db.Country.Select(x => new SelectListItem()
            {
                Value = x.CountryId.ToString(),
                Text = x.Name
            }).ToList();

            model.Countries.Insert(0, new SelectListItem { Value = "", Text = "Please select country of your address" });

            model.StudyCycles = new List<SelectListItem>();

            model.StudyCycles.Add(new SelectListItem()
            {
                Value = "",
                Text = "Please select your study cycle"
            });

            model.StudyCycles.Add(new SelectListItem()
            {
                Value = "Bachelor",
                Text = "Bachelor"
            });

            model.StudyCycles.Add(new SelectListItem()
            {
                Value = "Master",
                Text = "Master"
            });

            model.StudyTerms = new List<SelectListItem>();

            model.StudyTerms.Add(new SelectListItem()
            {
                Value = "",
                Text = "Please select your study term"
            });

            model.StudyTerms.Add(new SelectListItem()
            {
                Value = "1st semester",
                Text = "1st semester"
            });

            model.StudyTerms.Add(new SelectListItem()
            {
                Value = "2nd semester",
                Text = "2nd semester"
            });

            model.StudyTerms.Add(new SelectListItem()
            {
                Value = "3rd semester",
                Text = "3rd semester"
            });

            model.StudyTerms.Add(new SelectListItem()
            {
                Value = "4th semester",
                Text = "4th semester"
            });

            model.StudyTerms.Add(new SelectListItem()
            {
                Value = "5th semester",
                Text = "5th semester"
            });

            model.StudyTerms.Add(new SelectListItem()
            {
                Value = "6th semester",
                Text = "6th semester"
            });

            model.StudyTerms.Add(new SelectListItem()
            {
                Value = "7th semester",
                Text = "7th semester"
            });


            model.LangProfiency = new List<SelectListItem>();

            model.LangProfiency.Add(new SelectListItem()
            {
                Value = "",
                Text = "Please select proficiency for selected foreign language"
            });

            model.LangProfiency.Add(new SelectListItem()
            {
                Value = "A1 (Basic user)",
                Text = "A1 (Basic user)"
            });
            model.LangProfiency.Add(new SelectListItem()
            {
                Value = "A2 (Basic user)",
                Text = "A2 (Basic user)"
            });
            model.LangProfiency.Add(new SelectListItem()
            {
                Value = "B1 (Independent user)",
                Text = "B1 (Independent user)"
            });
            model.LangProfiency.Add(new SelectListItem()
            {
                Value = "B2 (Independent user)",
                Text = "B2 (Independent user)"
            });
            model.LangProfiency.Add(new SelectListItem()
            {
                Value = "C1 (Proficient user)",
                Text = "C1 (Proficient user)"
            });
            model.LangProfiency.Add(new SelectListItem()
            {
                Value = "C2 (Proficient user)",
                Text = "C2 (Proficient user)"
            });
            return model;
        }
    }
}