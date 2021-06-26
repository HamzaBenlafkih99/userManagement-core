using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Please.Models
{
    public class UserInfo
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        public string Role { get; set; }
    }
}
