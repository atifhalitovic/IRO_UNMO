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
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _service;

        public CountryController(ICountryService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Model.Country> Get([FromQuery]Model.Requests.CountrySearchRequest request)
        {
            return _service.Get(request);
        }

        [HttpPost]
        public Model.Country Insert([FromBody]Model.Requests.CountryInsertRequest request)
        {
            return _service.Insert(request);
        }
        [HttpPut("{id}")]
        public Model.Country Update(int id, [FromBody]Model.Requests.CountryInsertRequest request)
        {
            return _service.Update(id, request);
        }

        [HttpGet("{id}")]
        public Model.Country GetById(int id)
        {
            return _service.GetById(id);
        }
    }
}