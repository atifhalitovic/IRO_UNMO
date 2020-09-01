﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRO_UNMO.Model;
using IRO_UNMO.Model.Requests;

namespace IRO_UNMO.WebAPI.Services
{
    public interface IApplicantService
    {
        List<Model.Applicant> Get(Model.Requests.ApplicantSearchRequest request);
        Model.Applicant Insert(Model.Requests.ApplicantInsertRequest request);
        Applicant GetById(string id);
    }
}
