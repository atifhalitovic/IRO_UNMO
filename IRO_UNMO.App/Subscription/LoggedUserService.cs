using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IRO_UNMO.App.Data;
using IRO_UNMO.App.Models;
using IRO_UNMO.App.ViewModels;

namespace IRO_UNMO.App.Subscription
{
    public class LoggedUserService : ILoggedUser
    {
        private ApplicationDbContext _db;

        public LoggedUserService(ApplicationDbContext db)
        {
            _db = db;
        }
        public LoggedUserVM GetLoggedUser(string id)
        {
            LoggedUserVM model = _db.Users.Where(x => x.Id == id).Select(x => new LoggedUserVM()
            {
                Ime = x.Name,
                Prezime = x.Surname
            }).FirstOrDefault();
            return model;
        }

        //public LoggedUserSlikaVM GetLoggedUserSlika(int id)
        //{
        //    LoggedUserSlikaVM model = new LoggedUserSlikaVM();
        //    int _UserID = _db.Prijevoznik.Where(x => x.UserID == id).Select(x => x.PrijevoznikID).FirstOrDefault();
        //    string UserID = _UserID.ToString();

        //    List<Slike> _slike = _db.Slike.ToList();
        //    for (int y = 0; y < _slike.Count(); y++)
        //    {
        //        char temp = '0';
        //        temp = _slike[y].Naziv.LastOrDefault();
        //        if (temp.ToString() == UserID)
        //        {
        //            int tempID = _slike[y].SlikeID;
        //            model = _db.Slike.Where(s=>s.SlikeID==tempID).Select(x => new LoggedUserSlikaVM()
        //            {
        //                Naziv = x.Naziv,
        //                URL = x.URL,
        //                SlikaID=x.SlikeID
        //            }).FirstOrDefault();
        //        }
        //    }
        //    return model;
        //}
    }
}
