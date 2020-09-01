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
            var query = _context.Applicant.Include(x=>x.ApplicationUser).ThenInclude(y=>y.Country).Include(q=>q.University).AsQueryable();

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

        public Model.Applicant Insert(ApplicantInsertRequest request)
        {
            Database.Applicant entity = _mapper.Map<Database.Applicant>(request);

            //ApplicationUser user = new ApplicationUser
            //{
            //    Name = model.Name,
            //    Surname = model.Surname,
            //    Email = model.Email,
            //    PhoneNumber = model.PhoneNumber,
            //    CountryId = model.CountryId,
            //    UserName = model.Name.ToLower() + '.' + model.Surname.ToLower(),
            //    UniqueCode = GetRandomizedString(brojac),
            //    LastLogin = DateTime.Now
            //};

            //await _userManager.CreateAsync(user, password);

            //await _userManager.AddToRoleAsync(user, "IncomingApplicant");

            //Applicant applicant = new Applicant
            //{
            //    ApplicantId = user.Id,
            //    CreatedProfile = DateTime.Now,
            //    UniversityId = model.UniversityId,
            //    StudyCycle = model.StudyCycle,
            //    StudyField = model.StudyField,
            //    Verified = false,
            //    TypeOfApplication = model.TypeOfApplication
            //};

            //_db.Applicant.Add(applicant);
            //_db.SaveChanges();

            //if (request.Lozinka != request.LozinkaPotvrda)
            //{
            //    throw new Exception("Passwordi se ne slažu");
            //}

            //entity.LozinkaSalt = GenerateSalt();
            //entity.LozinkaHash = GenerateHash(entity.LozinkaSalt, request.Lozinka);

            //_context.Applicant.Add(entity);
            //_context.SaveChanges();

            return _mapper.Map<Model.Applicant>(entity);
        }

        public static string GenerateSalt()
        {
            var buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }

        public static string GenerateHash(string salt, string password)
        {
            byte[] src = Convert.FromBase64String(salt);
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] dst = new byte[src.Length + bytes.Length];

            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }

        public Model.Applicant GetById(string id)
        {
            var query = _context.Applicant.AsQueryable();

            query = query.Where(x => x.ApplicantId == id);

            //query = query.Include(x => x.Grad);
            //query = query.Include(x => x.Uloga);

            var entity = query.FirstOrDefault();

            return _mapper.Map<Model.Applicant>(entity);
        }
    }
}
