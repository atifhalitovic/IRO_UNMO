using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRO_UNMO.Model;
using IRO_UNMO.Model.Requests;

namespace IRO_UNMO.WebAPI.Services
{
    public interface IUniversityService
    {
        List<Model.University> Get(Model.Requests.UniversitySearchRequest request);
    }
}
