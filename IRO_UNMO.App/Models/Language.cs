using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class Language
    {
        public int LanguageId { get; set; }
        public string Native { get; set; }
        public string ForeignFirst { get; set; }
        public string ForeignFirstProficiency { get; set; }
        public string ForeignSecond { get; set; }
        public string ForeignSecondProficiency { get; set; }
        public string ForeignThird { get; set; }
        public string ForeignThirdProficiency { get; set; }
        public int ForeignExperienceNumber { get; set; }
    }
}
