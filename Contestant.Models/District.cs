using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text;

namespace Contestant.Models
{
    [Table("District")]
    public class District : BaseModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int StateId { get; set; }

        [Required]
        [StringLength(50)]
        public string DistrictName { get; set; }

        [StringLength(50)]
        public string DistrictNameLocLang { get; set; }

        [ForeignKey("StateId")]
        public State State { get; set; }
    }
}
