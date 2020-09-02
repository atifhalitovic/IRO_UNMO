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
    public class UsersService : IUsersService
    {
        private readonly IRO_UNMO_Context _context;

        private readonly IMapper _mapper;

        public UsersService(IRO_UNMO_Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Model.Applicant> Get(ApplicantSearchRequest request)
        {
            var query = _context.Applicant.Include(x => x.ApplicationUser).ThenInclude(y => y.Country).Include(q => q.University).AsQueryable();

            //if (!string.IsNullOrWhiteSpace(request?.ImePrezime))
            //{
            //    query = query.Where(x => (x.Ime + " " + x.Prezime).ToLower().Contains(request.ImePrezime.ToLower()) ||
            //                              x.KorisnickoIme.Contains(request.ImePrezime.ToLower()));
            //}

            //if (!string.IsNullOrWhiteSpace(request?.UserName))
            //{
            //    query = query.Where(x => x.KorisnickoIme.ToLower().StartsWith(request.UserName));
            //}

            //query = query.Include(x => x.Grad.Drzava);
            //query = query.Include(x => x.Uloga);

            var list = query.ToList();

            return _mapper.Map<List<Model.Applicant>>(list);
        }

        public Model.Applicant Insert(ApplicantInsertRequest request)
        {
            Database.Applicant entity = _mapper.Map<Database.Applicant>(request);
            if (request.Lozinka != request.LozinkaPotvrda)
            {
                throw new Exception("Passwordi se ne slažu");
            }

            entity.LozinkaSalt = GenerateSalt();
            entity.LozinkaHash = GenerateHash(entity.LozinkaSalt, request.Lozinka);

            _context.Applicant.Add(entity);
            _context.SaveChanges();

            return _mapper.Map<Model.Applicant>(entity);
        }


        public Model.Applicant UpdateProfile(ApplicantUpdateProfilRequest request)
        {
            int KorisnikId = Security.BasicAuthenticationHandler.PrijavljeniKorisnik.ApplicantId;

            Database.Applicant entity = _context.Applicant.Where(x => x.ApplicantId == KorisnikId).FirstOrDefault();

            _context.Applicant.Attach(entity);
            _context.Applicant.Update(entity);

            if (!string.IsNullOrEmpty(request.Lozinka))
            {
                if (request.Lozinka != request.LozinkaPotvrda)
                {
                    throw new Exception("Passwordi se ne slažu");
                }

                entity.LozinkaSalt = GenerateSalt();
                entity.LozinkaHash = GenerateHash(entity.LozinkaSalt, request.Lozinka);
            }

            entity = _mapper.Map(request, entity);

            _context.SaveChanges();

            return _mapper.Map<Model.Applicant>(entity);
        }


        public Model.Applicant Authenticiraj(string username, string pass)
        {
            var user = _context.Applicant
                .Include(x => x.Uloga)
                .Include(x => x.Grad.Drzava)
                .FirstOrDefault(x => x.KorisnickoIme == username);

            if (user != null)
            {
                var newHash = GenerateHash(user.LozinkaSalt, pass);

                if (newHash == user.LozinkaHash)
                {
                    return _mapper.Map<Model.Applicant>(user);
                }
            }
            return null;
        }
    }
}
