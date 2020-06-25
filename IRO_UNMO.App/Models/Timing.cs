using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class Timing
    {
        public int TimingId { get; set; }
        public string Semester { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
