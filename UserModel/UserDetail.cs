using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace AuthenticationService.UserModel
{
    public class UserDetail
    {
        [Key]
        public int UserId { get; set; }

        //[Column(TypeName ="nvarchar(100")]
        [Required]
        public string UserName { get; set; }

        //[Column(TypeName = "nvarchar(8")]
        [Required]
        public string Password { get; set; } 


      
    }
}
