using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRO_UNMO.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IRO_UNMO.WebAPI.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _service;

        public ApplicantController(IApplicantService service)
        {
            _service = service;
        }

        [HttpGet]
        //[Authorize]
        public List<Model.Applicant> Get([FromQuery]Model.Requests.ApplicantSearchRequest request)
        {
            return _service.Get(request);
        }
    }
}