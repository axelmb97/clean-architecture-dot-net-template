using Application.Common.Abstractions.Services;

namespace Infraestructure.Services
{
    public class AuthCachedService : IAuthCachedService
    {
        private string UserId { get; set; }

        public AuthCachedService()
        {
            
        }

        public string? Pop()
        {
            if (string.IsNullOrEmpty(UserId)) throw new UnauthorizedAccessException();
            return UserId;
        }

        public void Push(string userId)
        {
            UserId = userId;
        }
    }
}
