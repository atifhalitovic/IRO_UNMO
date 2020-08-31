using System;
using System.Collections.Generic;

namespace IRO_UNMO.Model
{
    public partial class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
