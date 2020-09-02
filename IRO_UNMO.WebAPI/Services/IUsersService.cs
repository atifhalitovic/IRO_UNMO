using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRO_UNMO.Model;
using IRO_UNMO.Model.Requests;

namespace IRO_UNMO.WebAPI.Services
{
    public interface IUsersService
    {
        List<Model.Applicant> Get(Model.Requests.ApplicantSearchRequest request);
        Model.Applicant Insert(Model.Requests.ApplicantInsertRequest request);
        Applicant GetById(int id);
        Applicant MyProfile();
        Model.Applicant Authenticiraj(string username, string pass);
    }
}
