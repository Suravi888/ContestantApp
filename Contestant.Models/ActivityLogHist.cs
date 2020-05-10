using Contestant.ICommon.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contestant.Models
{
    public class ActivityLogHist
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ActivityLogId { get; set; }

        [Required]
        public RowStatusOption Status { get; set; }

        [Required]
        [Column(TypeName = "datetime"), DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime StatusChgDate { get; set; }

        [Required]
        public int StatusChgUserID { get; set; }

        [Required]
        [StringLength(200)]
        public string Remarks { get; set; }

        [Required]
        public string DataJson { get; set; }

        [ForeignKey("ActivityLogId")]
        public ActivityLog ActivityLog { get; set; }

        [ForeignKey("StatusChgUserID")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
