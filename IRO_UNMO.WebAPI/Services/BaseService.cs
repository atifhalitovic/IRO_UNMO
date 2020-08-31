using AutoMapper;
using IRO_UNMO.WebAPI.Database;
using System.Collections.Generic;
using System.Linq;

namespace IRO_UNMO.WebAPI.Services
{
    public class BaseService<TModel, TSearch, TDatabase> : IService<TModel, TSearch> where TDatabase : class
    {
        protected readonly IRO_UNMO_Context db;
        protected readonly IMapper mapper;
        public BaseService(IRO_UNMO_Context _db, IMapper m)
        {
            db = _db;
            mapper = m;
        }
        public virtual List<TModel> Get(TSearch search)
        {
            var list = db.Set<TDatabase>().ToList();
            return mapper.Map<List<TModel>>(list);
        }

        public virtual TModel GetById(int id)
        {
            var model = db.Set<TDatabase>().Find(id);
            return mapper.Map<TModel>(model);
        }
    }
}