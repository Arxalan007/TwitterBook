﻿using System.Collections.Generic;

namespace TwitterBook.Contracts.Responses
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}