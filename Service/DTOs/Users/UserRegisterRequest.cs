using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Users
{
    public class UserRegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public DateTime DOB { get; set; }
    }
}
