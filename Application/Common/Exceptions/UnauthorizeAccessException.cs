using Application.Common.Exceptions.Base;
using Application.Common.Resources;

namespace Application.Common.Exceptions
{
    public class UnauthorizeAccessException : BaseException
    {
        private static string errorMessage = Messages.UnauthorizeAccessMessage;
        public UnauthorizeAccessException() : base(errorMessage)
        {
        }
    }
}
