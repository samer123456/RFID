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

    public class SettingsModel
    {
        public string ApiUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PortName { get; set; }
        public string BaudRate { get; set; }
        public string Parity { get; set; }
        public string DataBits { get; set; }
        public string StopBits { get; set; }
    }
} 