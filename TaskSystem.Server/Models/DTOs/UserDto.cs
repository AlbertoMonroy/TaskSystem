using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskSystem.Server.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

}