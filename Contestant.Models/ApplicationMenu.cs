using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contestant.Models
{
    public class ApplicationMenu
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int? MastMenuId { get; set; }

        //[Required]
        //public MenuVersion MenuVersion { get; set; }

        [Required]
        [StringLength(75)]
        public string MenuName { get; set; }

        [Required]
        public bool IsGroup { get; set; }

        [Required]
        public int MenuOrder { get; set; }

        [Required]
        [StringLength(50)]
        public string MenuIconClass { get; set; }

        [Required]
        [StringLength(50)]
        public string Area { get; set; }

        [Required]
        [StringLength(100)]
        public string Controller { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; }

        [Required]
        public bool IsHidden { get; set; }

        [Required]
        public bool RequiredActivityLog { get; set; }

        [Required]
        public bool RequiredApproval { get; set; }

        [ForeignKey("MastMenuId")]
        public ApplicationMenu MastMenu { get; set; }

        public List<ActivityLog> ActivityLog { get; set; }
    }
}
