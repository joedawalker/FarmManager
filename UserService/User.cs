﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace UserService
{
    public class User
    {
        [JsonPropertyName( "id" )]
        public int Id { get; set; }

        [JsonPropertyName( "first_name" )]
        public string FirstName { get; set; }

        [JsonPropertyName( "last_name" )]
        public string LastName { get; set; }

        [JsonPropertyName( "email" )]
        public string Email { get; set; }

        [JsonPropertyName( "pasword" )]
        public string Password { get; set; }
    }
}
