using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IRO_UNMO.WebAPI.Services;

namespace IRO_UNMO.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityService _service;

        public UniversityController(IUniversityService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Model.University> Get([FromQuery]Model.Requests.UniversitySearchRequest request)
        {
            return _service.Get(request);
        }
    }
}