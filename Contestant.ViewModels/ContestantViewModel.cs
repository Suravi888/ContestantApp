using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contestant.ViewModels
{
    public class ContestantViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "{0} must be at least {2} characters and at max {1} characters long", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "{0} must be at least {2} characters and at max {1} characters long", MinimumLength = 1)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Birth Date is required.")]
        [Display(Name = "Birth Date")]
        //[Date]
        public string DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "District is required.")]
        [Display(Name = "District Name")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid {0}. The {0} must be greater than {1}")]
        public int? DistrictId { get; set; }

        [Display(Name = "District Name")]
        public string DistrictName { get; set; }

        [Required]
        [Display(Name = "Gender")]
        [StringLength(20, ErrorMessage = "{0} must be at least {2} characters and at max {1} characters long", MinimumLength = 3)]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(100, ErrorMessage = "{0} must be atleast {2} characters and at max {1} characters long", MinimumLength = 3)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Photo")]
        [StringLength(50)]
        public string PhotoUrl { get; set; }
    }
}
