using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRO_UNMO.App.Models
{
    public class Token
    {
        [Key]
        public int TokenId { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(50)")]
        public string Value { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Column(TypeName = "SMALLDATETIME")]
        public DateTime Created { get; set; }
    }
}