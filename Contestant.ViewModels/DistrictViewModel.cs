using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contestant.ViewModels
{
    public class DistrictViewModel : BaseViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [Display(Name = "State")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid {0}. The {0} must be greater than {1}")]
        public int? StateId { get; set; }

        [Display(Name = "State")]
        public string StateName { get; set; }

        [Required]
        [Display(Name = "District Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string DistrictName { get; set; }

        [Required]
        [Display(Name = "District Name (Local)")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string DistrictNameLocLang { get; set; }
    }
}
