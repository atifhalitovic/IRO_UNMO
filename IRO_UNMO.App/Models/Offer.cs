using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }
        public int UniversityId { get; set; }
        public string ProgrammeName { get; set; }
        public string StudyLanguage { get; set; }

        public DateTime Start
        {
            get;
            set;
        }

        public DateTime End
        {
            get;
            set;
        }
    }
}
