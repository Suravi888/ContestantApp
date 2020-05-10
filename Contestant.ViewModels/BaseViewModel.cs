using Contestant.ICommon.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contestant.ViewModels
{
    public class BaseViewModel
    {
        [Display(Name = "Status")]
        public RowStatusOption Status { get; set; }

        [Display(Name = "Date")]
       // [Date]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public string StatusChgDate { get; set; }

        [Display(Name = "User Id", Order = 103)]
        public int StatusChgUserID { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 3)]
        [Display(Name = "User Name")]
        public string StatusChgUserName { get; set; }
    }
}
