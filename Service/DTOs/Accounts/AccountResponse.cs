﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Accounts
{
    public class AccountResponse
    {
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
