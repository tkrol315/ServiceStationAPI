namespace ServiceStationAPI
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; }
        public int JwtExpireMins { get; set; }
        public string JwtIssuer { get; set; }
    }
}
