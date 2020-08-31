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
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseCRUDController<TModel, TSearch, TInsert, TUpdate> : BaseController<TModel, TSearch>
    {
        private readonly ICRUDService<TModel, TSearch, TInsert, TUpdate> service = null;
        public BaseCRUDController(ICRUDService<TModel, TSearch, TInsert, TUpdate> _service) : base(_service)
        {
            service = _service;
        }

        [HttpPost]
        public TModel Insert(TInsert insert)
        {
            var result = service.Insert(insert);
            return result;
        }

        [HttpPut("{id}")]
        public ActionResult<TModel> Update(int id, TUpdate update)
        {
            var result = service.Update(id, update);
            return result;
        }

        [HttpDelete("{id}")]
        public bool Remove(int id)
        {
            return service.Remove(id);
        }
    }
}