using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contestant.Models
{
    [Table("Contestant")]
    public class Contestants
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "FirstName Required")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName Required")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "Date"), DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public bool IsActive { get; set; }


        [Required]
        public int DistrictId { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        [StringLength(20)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Address Required")]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string PhotoUrl { get; set; }

        [ForeignKey("DistrictId")]
        public District District { get; set; }
    }
}
