using Contestant.ICommon.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contestant.Models
{
    public abstract class BaseModel
    {
        [Required]
        public RowStatusOption  Status { get; set; }

        [Required]
        public int EntryUserId { get; set; }

        [Required]
        [Column(TypeName = "Date"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime EntryDate { get; set; }

        [Required]
        public int StatusChgUserID { get; set; }

        [Required]
        [Column(TypeName = "datetime"), DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime StatusChgDate { get; set; }
        
        //Foreign Key
        [ForeignKey("EntryUserId")]
        public virtual ApplicationUser EntryUserApplicationUser { get; set; }

        [ForeignKey("StatusChgUserID")]
        public virtual ApplicationUser StatusChgUserApplicationUser { get; set; }
    }
}
