using AutoMapper;
using IRO_UNMO.WebAPI.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace IRO_UNMO.WebAPI.Services
{
    public class BaseCRUDService<TModel, TSearch, TDatabase, TInsert, TUpdate> : BaseService<TModel, TSearch, TDatabase>, ICRUDService<TModel, TSearch, TInsert, TUpdate> where TDatabase : class
    {
        public BaseCRUDService(IRO_UNMO_Context _db, IMapper m) : base(_db, m)
        {
        }

        public virtual TModel Insert(TInsert request)
        {
            var entity = mapper.Map<TDatabase>(request);
            db.Add(entity);
            db.SaveChanges();
            return mapper.Map<TModel>(entity);
        }

        public bool Remove(int id)
        {
            var entity = db.Set<TDatabase>().Find(id);
            if (entity != null)
            {
                db.Set<TDatabase>().Remove(entity);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public virtual TModel Update(int id, TUpdate request)
        {
            var entity = db.Set<TDatabase>().Find(id);
            mapper.Map(request, entity);
            db.SaveChanges();
            return mapper.Map<TModel>(entity);
        }
    }
}