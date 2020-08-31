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
            CreateMap<IRO_UNMO.WebAPI.Database.Country, Model.Country>().ReverseMap();
            CreateMap<IRO_UNMO.WebAPI.Database.Country, CountryRequest>().ReverseMap();
        }
    }
}
