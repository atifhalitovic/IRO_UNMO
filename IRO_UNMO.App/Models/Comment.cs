using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string AdministratorId { get; set; }
        public Administrator Administrator { get; set; }
        public string ApplicantId { get; set; }
        public Applicant Applicant { get; set; }
        public int IonId { get; set; }
        public string Message { get; set; }
        public DateTime CommentTime { get; set; }
    }
}
