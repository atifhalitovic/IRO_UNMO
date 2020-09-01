using System;
using System.Collections.Generic;
using System.Text;

namespace IRO_UNMO.Model.Requests
{
    public class ApplicantSearchRequest
    {
        public string ImePrezime { get; set; }
        public string UserName { get; set; }
        public bool Aktivan { get; set; }
    }
}
