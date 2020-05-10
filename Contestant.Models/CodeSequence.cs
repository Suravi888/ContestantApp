using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contestant.Models
{
    public class CodeSequence
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(75)]
        public string SequenceKey { get; set; }

        [Required]
        [StringLength(50)]
        public string ChangeSequenceKey { get; set; }

        [Required]
        public int SequenceValue { get; set; }
    }
}
