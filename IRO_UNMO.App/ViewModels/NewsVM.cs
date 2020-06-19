using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.ViewModels
{
    public class NewsVM
    {
        public List<Comment> Comments { get; set; }
    }
}
