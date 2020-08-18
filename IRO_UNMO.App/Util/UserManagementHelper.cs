using IRO_UNMO.App.Areas.Admin.ViewModels;
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

        public EditOffersVM prepOffer()
        {
            EditOffersVM model = new EditOffersVM();

            model.Universities = _db.University.Include(a => a.Country).Where(b => b.UniversityId != 2).Select(x => new SelectListItem()
            {
                Value = x.UniversityId.ToString(),
                Text = x.Name
            }).OrderBy(a => a.Value).ToList();

            model.Universities.Insert(0, new SelectListItem { Value = "", Text = "" });

            model.Semesters = new List<SelectListItem>();

            model.Semesters.Add(new SelectListItem()
            {
                Value = "",
                Text = ""
            });

            model.Semesters.Add(new SelectListItem()
            {
                Value = "Erasmus+, summer",
                Text = "Erasmus+, summer"
            });

            model.Semesters.Add(new SelectListItem()
            {
                Value = "Erasmus+, winter",
                Text = "Erasmus+, winter"
            });

            model.Semesters.Add(new SelectListItem()
            {
                Value = "other",
                Text = "other"
            });

            model.Programmes = new List<SelectListItem>();

            model.Programmes.Add(new SelectListItem()
            {
                Value = "other",
                Text = "other"
            });

            model.Programmes.Add(new SelectListItem()
            {
                Value = "mechanic engineering",
                Text = "mechanics engineering"
            });

            model.Programmes.Add(new SelectListItem()
            {
                Value = "civil engineering",
                Text = "civil engineering"
            });

            model.Programmes.Add(new SelectListItem()
            {
                Value = "IT",
                Text = "IT"
            });

            model.Programmes.Add(new SelectListItem()
            {
                Value = "business administration",
                Text = "business administration"
            });

            model.Programmes.Add(new SelectListItem()
            {
                Value = "available in info link",
                Text = "available in info link"
            });

            model.Cycles = new List<SelectListItem>();

            model.Cycles.Add(new SelectListItem()
            {
                Value = "bachelor",
                Text = "bachelor"
            });

            model.Cycles.Add(new SelectListItem()
            {
                Value = "master",
                Text = "master"
            });

            model.Cycles.Add(new SelectListItem()
            {
                Value = "doctoral",
                Text = "doctoral"
            });

            model.Cycles.Add(new SelectListItem()
            {
                Value = "other",
                Text = "other"
            });

            return model;
        }
        public UsersVM prepUser()
        {
            UsersVM model = new UsersVM();

            model.Countries = new List<SelectListItem>();

            model.Countries = _db.Country.Select(x => new SelectListItem()
            {
                Value = x.CountryId.ToString(),
                Text = x.Name
            }).OrderBy(a=>a.Text).ToList();

            model.Countries.Insert(0, new SelectListItem { Value = "", Text = "" });    

            model.Universities = new List<SelectListItem>();

            model.Universities = _db.University.Where(a => a.UniversityId != 2).Select(x => new SelectListItem()
            {
                Value = x.UniversityId.ToString(),
                Text = x.Name
            }).ToList();

            model.Universities.Insert(0, new SelectListItem { Value = "", Text = "" });

            model.TypesOfApplicant = new List<SelectListItem>();

            model.TypesOfApplicant.Add(new SelectListItem()
            {
                Value = "",
                Text = ""
            });

            model.TypesOfApplicant.Add(new SelectListItem()
            {
                Value = "student",
                Text = "student"
            });

            model.TypesOfApplicant.Add(new SelectListItem()
            {
                Value = "staff",
                Text = "staff"
            });

            model.TypesOfApplicant.Add(new SelectListItem()
            {
                Value = "other",
                Text = "other"
            });

            model.StudyCycles = new List<SelectListItem>();

            model.StudyCycles.Add(new SelectListItem()
            {
                Value = "",
                Text = ""
            });

            model.StudyCycles.Add(new SelectListItem()
            {
                Value = "bachelor",
                Text = "bachelor"
            });

            model.StudyCycles.Add(new SelectListItem()
            {
                Value = "master",
                Text = "master"
            });


            model.StudyCycles.Add(new SelectListItem()
            {
                Value = "doctoral",
                Text = "doctoral"
            });

            model.StudyCycles.Add(new SelectListItem()
            {
                Value = "other",
                Text = "other"
            });

            model.Faculties = new List<SelectListItem>();

            model.Faculties.Add(new SelectListItem()
            {
                Value = "",
                Text = ""
            });

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
                Text = ""
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
                Text = ""
            });

            model.Genders.Add(new SelectListItem()
            {
                Value = "male",
                Text = "male"
            });

            model.Genders.Add(new SelectListItem()
            {
                Value = "female",
                Text = "female"
            });

            model.Genders.Add(new SelectListItem()
            {
                Value = "not specified",
                Text = "not specified"
            });

            model.Countries = _db.Country.Select(x => new SelectListItem()
            {
                Value = x.CountryId.ToString(),
                Text = x.Name
            }).ToList();

            model.Countries.Insert(0, new SelectListItem { Value = "", Text = "" });

            model.StudyCycles = new List<SelectListItem>();

            model.StudyCycles.Add(new SelectListItem()
            {
                Value = "",
                Text = ""
            });

            model.StudyCycles.Add(new SelectListItem()
            {
                Value = "bachelor",
                Text = "bachelor"
            });

            model.StudyCycles.Add(new SelectListItem()
            {
                Value = "master",
                Text = "master"
            });

            model.StudyTerms = new List<SelectListItem>();

            model.StudyTerms.Add(new SelectListItem()
            {
                Value = "",
                Text = ""
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
                Text = ""
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