using Contestant.ICommon.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;

namespace Contestant.ViewModels
{
    public class ActivityLogViewModel
    {
        [Required]
        public int RelatedId { get; set; }

        [Required]
        public int MenuId { get; set; }

        [Required]
        public RowStatusOption Status { get; set; }

        [Required]
        public int StatusChgUserID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime StatusChgDate { get; set; }

        [Required]
        public string Remarks { get; set; }

        public string DataJson { get; set; }
    }
}
