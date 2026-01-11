namespace Server.Network
{
    public class AuthCommand
    {
        public string Type { get; set; }
        public string Token { get; set; }
    }

    public class AdminCommand
    {
        public string Type { get; set; }
        public string TargetUserId { get; set; }
        public string Message { get; set; }
    }
}
