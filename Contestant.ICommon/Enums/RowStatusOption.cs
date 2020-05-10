using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contestant.ICommon.Enums
{
    public enum RowStatusOption
    {
        New = 1,
        Approved = 2,
        Modified = 3, 
        Deleted = 4,

        [Display(Name = "Approved Modified")]
        ApprovedModified = 5,

        [Display(Name = "Non Approval")]
        NonApproval = 6,
    }
}
