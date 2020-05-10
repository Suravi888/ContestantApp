using System;
using System.Collections.Generic;
using System.Text;

namespace Contestant.ViewModels
{
    public class ActionPermissionViewModel
    {
        public bool Create { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool View { get; set; }

        public bool Approve { get; set; }
        public bool ApproveDelete { get; set; }
        public bool ApproveModify { get; set; }
        public bool Print { get; set; }
    }
}
