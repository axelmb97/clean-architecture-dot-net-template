using Application.Common.Dtos.Errors;

namespace Application.Common.Exceptions.Base
{
    public class ValidationBaseException : BaseException
    {
        public IEnumerable<ValidationErrorDto> Errors { get; set; }
        public ValidationBaseException(IEnumerable<ValidationErrorDto> errors) : base(BuildErrorMessage(errors))
        {
        }

        private static string BuildErrorMessage(IEnumerable<ValidationErrorDto> errors)
        {
            if (!errors.Any()) return "";

            return string.Join(';', errors.Select(e => $"Property {e.PropertyName} has the following erros: {BuildErrorList(e.Errors)}"));
        }

        private static string BuildErrorList(IEnumerable<string> errors)
        {
            if (!errors.Any()) return "No errors";

            return string.Join("\n", errors.Select(e => $"* {e}"));
        }
    }
}
