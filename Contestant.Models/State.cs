using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contestant.Models
{
    public class State : BaseModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string StateName { get; set; }

        [StringLength(100)]
        public string StateNameLocLang { get; set; }
    }
}
