using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class HomeInstitution
    {
        public int HomeInstitutionId { get; set; }
        public string OfficialName { get; set; }
        public string DepartmentName { get; set; }
        public string LevelOfEducation { get; set; }
        public string CurrentTermOrYear { get; set; }
        public string StudyProgramme { get; set; }
        public string CoordinatorFullName { get; set; }
        public string CoordinatorPosition { get; set; }
        public string CoordinatorEmail { get; set; }
        public string CoordinatorAddress { get; set; }
        public string CoordinatorPhoneNum { get; set; }
    }
}
