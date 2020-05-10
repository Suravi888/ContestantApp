using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contestant.Models
{
    public class ExceptionLog
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        [StringLength(500)]
        public string InnerMessage { get; set; }

        [StringLength(500)]
        public string Source { get; set; }

        public string StackTrace { get; set; }

        public DateTime DateTime { get; set; }

        [StringLength(300)]
        public string ModuleName { get; set; }

        [StringLength(300)]
        public string ActionName { get; set; }
    }
}
