﻿using System.ComponentModel.DataAnnotations;

namespace TwitterBook.Contracts.Requests
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}