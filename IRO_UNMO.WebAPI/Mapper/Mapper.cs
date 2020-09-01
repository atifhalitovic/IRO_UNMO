using AutoMapper;
using IRO_UNMO.Model;
using IRO_UNMO.Model.Requests;
using IRO_UNMO.WebAPI.Database;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.WebAPI.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Database.Country, Model.Country>();
            CreateMap<Database.Applicant, Model.Applicant>();
            CreateMap<Database.ApplicationUser, Model.ApplicationUser>();
            CreateMap<Database.University, Model.University>();
            CreateMap<Database.Country, Model.Country>().ReverseMap();
            CreateMap<Database.Applicant, Model.Applicant>().ReverseMap();
            CreateMap<Database.University, Model.University>().ReverseMap();
            CreateMap<Database.ApplicationUser, Model.ApplicationUser>().ReverseMap();
            CreateMap<Database.Country, Model.Requests.CountryInsertRequest>().ReverseMap();
            CreateMap<Database.Applicant, Model.Requests.ApplicantInsertRequest>().ReverseMap();
            CreateMap<Database.Applicant, Model.Requests.ApplicantSearchRequest>().ReverseMap();
        }
    }
}
