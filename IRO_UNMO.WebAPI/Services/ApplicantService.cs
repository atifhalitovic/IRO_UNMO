using AutoMapper;
using IRO_UNMO.Model;
using IRO_UNMO.Model.Requests;
using IRO_UNMO.WebAPI.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IRO_UNMO.WebAPI.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly IRO_UNMO_Context _context;

        private readonly IMapper _mapper;

        public ApplicantService(IRO_UNMO_Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Model.Applicant> Get(ApplicantSearchRequest request)
        {
            var query = _context.Applicant.Include(x => x.ApplicationUser).ThenInclude(y => y.Country).Include(q => q.University).AsQueryable();

            //if (!string.IsNullOrWhiteSpace(request?.ImePrezime))
            //{
            //    query = query.Where(x => (x.ApplicationUser.Name + " " + x.ApplicationUser.Surname).ToLower().Contains(request.ImePrezime.ToLower()) ||
            //                              x.ApplicationUser.UserName.Contains(request.ImePrezime.ToLower()));
            //}

            //if (!string.IsNullOrWhiteSpace(request?.UserName))
            //{
            //    query = query.Where(x => x.ApplicationUser.UserName.ToLower().StartsWith(request.UserName));
            //}

            var list = query.ToList();

            return _mapper.Map<List<Model.Applicant>>(list);
        }

        public Model.Applicant GetById(string id)
        {
            var query = _context.Applicant.AsQueryable();

            query = query.Where(x => x.ApplicantId == id);

            var entity = query.FirstOrDefault();

            return _mapper.Map<Model.Applicant>(entity);
        }

        public Model.Applicant Authenticiraj(string uniqueCode)
        {
            var user = _context.Applicant.Include(a => a.ApplicationUser).Where(a => a.ApplicationUser.UniqueCode == uniqueCode).FirstOrDefault();

            if (user != null)
            {
                return _mapper.Map<Model.Applicant>(user);
            }
            return null;
        }
    }
}
