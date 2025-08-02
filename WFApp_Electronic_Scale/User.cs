using System;

namespace WFApp_Electronic_Scale
{
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; } = "User"; // Admin أو User
    }
} 