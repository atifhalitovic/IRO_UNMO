using AutoMapper;
using IRO_UNMO.Model.Requests;
using IRO_UNMO.WebAPI.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.WebAPI.Services
{
    public class CountryService : ICountryService
    {
        private readonly IRO_UNMO_Context _context;

        private readonly IMapper _mapper;

        public CountryService(IRO_UNMO_Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Model.Country> Get(CountrySearchRequest request)
        {
            var query = _context.Country.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request?.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(request.Name.ToLower()));
            }

            var list = query.ToList();

            return _mapper.Map<List<Model.Country>>(list);
        }

        public Model.Country Insert(CountryInsertRequest request)
        {
            Database.Country entity = _mapper.Map<Database.Country>(request);

            _context.Country.Add(entity);
            _context.SaveChanges();

            return _mapper.Map<Model.Country>(entity);
        }
        public Model.Country GetById(int id)
        {
            var query = _context.Country.AsQueryable();

            query = query.Where(x => x.CountryId == id);

            var entity = query.FirstOrDefault();

            return _mapper.Map<Model.Country>(entity);
        }

        public Model.Country Update(int id, CountryInsertRequest request)
        {
            Database.Country entity = _context.Country.Where(x => x.CountryId == id).FirstOrDefault();

            _context.Country.Attach(entity);
            _context.Country.Update(entity);

            entity = _mapper.Map(request, entity);

            _context.SaveChanges();

            return _mapper.Map<Model.Country>(entity);
        }
    }
}
