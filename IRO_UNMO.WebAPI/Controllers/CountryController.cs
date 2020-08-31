using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRO_UNMO.Model;
using IRO_UNMO.Model.Requests;
using IRO_UNMO.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace IRO_UNMO.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : BaseCRUDController<Model.Country, Model.Country, Model.Country, Model.Requests.CountryRequest>
    {
        public CountryController(ICRUDService<Model.Country, Model.Country, Model.Country, Model.Requests.CountryRequest> _service) : base(_service)
        {

        }
    }
}