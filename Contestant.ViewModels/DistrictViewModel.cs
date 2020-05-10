using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contestant.ViewModels
{
    public class DistrictViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "District Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Name { get; set; }
    }
}
