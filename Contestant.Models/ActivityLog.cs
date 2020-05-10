using Contestant.ICommon.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contestant.Models
{
    public class ActivityLog
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int MenuId { get; set; }

        [Required]
        public int RelatedId { get; set; }

        public RowStatusOption Status { get; set; }

        [Required]
        public int StatusChgUserID { get; set; }

        [Required]
        [Column(TypeName = "datetime") ,DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime StatusChgDate { get; set; }

        //Foreign Key
        [ForeignKey("MenuId")]
        public ApplicationMenu ApplicationMenu { get; set; }

        [ForeignKey("StatusChgUserID")]
        public int MyProperty { get; set; }

        public List<ActivityLogHist> ActivityLogHist { get; set; }
    }
}
