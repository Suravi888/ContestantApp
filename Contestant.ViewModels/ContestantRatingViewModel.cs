using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contestant.ViewModels
{
    public class ContestantRatingViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Contestant is required.")]
        [Display(Name = "Contestant")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid {0}. The {0} must be greater than {1}")]
        public int? ContestantId { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Display(Name = "Rating")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid {0}. The {0} must be between {1} and {2}")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Rated Date is required.")]
        [Display(Name = "Rated Date")]
        //[Date]
        public string RatedDate { get; set; }
    }
}
