using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Services.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}