using AutoMapper;
using IRO_UNMO.Model.Requests;
using IRO_UNMO.WebAPI.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.WebAPI.Services
{
    public class UniversityService : IUniversityService
    {
        private readonly IRO_UNMO_Context _context;

        private readonly IMapper _mapper;

        public UniversityService(IRO_UNMO_Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Model.University> Get(UniversitySearchRequest request)
        {
            var query = _context.University.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request?.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(request.Name.ToLower()));
            }

            var list = query.ToList();

            return _mapper.Map<List<Model.University>>(list);
        }
    }
}
