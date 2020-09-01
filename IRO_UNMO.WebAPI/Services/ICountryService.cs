using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRO_UNMO.Model;
using IRO_UNMO.Model.Requests;

namespace IRO_UNMO.WebAPI.Services
{
    public interface ICountryService
    {
        List<Model.Country> Get(Model.Requests.CountrySearchRequest request);
        Model.Country Insert(Model.Requests.CountryInsertRequest request);
        Country Update(int id, CountryInsertRequest request);
        Country GetById(int id);
    }
}
