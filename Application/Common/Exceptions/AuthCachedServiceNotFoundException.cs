using Application.Common.Exceptions.Base;
using Application.Common.Resources;

namespace Application.Common.Exceptions
{
    public class AuthCachedServiceNotFoundException : BaseException
    {
        private static string errorMessage = Messages.AuthCachedServiceNotFound;
        public AuthCachedServiceNotFoundException() : base(errorMessage)
        {
        }
    }
}
