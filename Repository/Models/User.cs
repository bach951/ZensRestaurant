using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public string Role { get; set; }
        public DateTime DOB { get; set; }
        public string Avatar { get; set; }
        public IEnumerable<RefreshToken> RefreshTokens { get; set; }
    }
}
