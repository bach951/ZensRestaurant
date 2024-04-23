using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Users
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public DateTime DOB { get; set; }
        public string Avatar { get; set; }
    }
}
