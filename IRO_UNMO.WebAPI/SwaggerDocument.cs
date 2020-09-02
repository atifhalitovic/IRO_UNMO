using System.Collections.Generic;

namespace IRO_UNMO.WebAPI
{
    public class SwaggerDocument
    {
        public Dictionary<string, IEnumerable<string>>[] Security { get; internal set; }
    }
}