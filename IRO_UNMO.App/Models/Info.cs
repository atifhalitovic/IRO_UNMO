using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class Info
    {
        public int InfoId { get; set; }
        public string Gender { get; set; }
        public int? CitizenshipId { get; set; }
        public Country Citizenship { get; set; }
        public int? SecondCitizenshipId { get; set; }
        public Country SecondCitizenship { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string PassportNumber { get; set; }
        public DateTime PassportIssueDate { get; set; }
        public DateTime PassportExpiryDate { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string MothersMaidenName { get; set; }
    }
}
