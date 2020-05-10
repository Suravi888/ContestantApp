using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contestant.Models
{
    [Table("ContestantRating")]
    public class ContestantRating
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ContestantId { get; set; }
        
        [Required]
        public int Rating { get; set; }

        [Required]
        [Column(TypeName = "Date"), DataType(DataType.Date)]
        public DateTime RatedDate { get; set; }

        [ForeignKey("ContestantId")]
        public Contestants Contestant { get; set; }
    }
}
