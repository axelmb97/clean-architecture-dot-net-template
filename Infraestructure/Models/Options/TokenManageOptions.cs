using Infraestructure.Resources;

namespace Infraestructure.Models.Options
{
    public class TokenManageOptions
    {
        public static string SectionName { get; set; } = CodeStrings.TokenSection;
        public string SecretKey { get; set; }
        public int ExpiresIn { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
