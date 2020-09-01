using System;

namespace IRO_UNMO.Model
{
    public class ApplicationUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UniqueCode { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public DateTime LastLogin { get; set; }
        public string SignalRToken { get; set; }
    }
}
