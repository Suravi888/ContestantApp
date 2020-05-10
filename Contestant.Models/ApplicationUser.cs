using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contestant.Models
{
    public class ApplicationUser /*: IdentityUser*/
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        [Required]
        [StringLength(300)]
        public string FullName { get; set; }

        //[Required]
        //public UserTypeOption UserType { get; set; }
        public int? UserMapId { get; set; }
    }
}
