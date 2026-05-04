namespace Application.Common.Abstractions.Services
{
    public interface IAuthCachedService
    {
        void Push(string userId);
        string? Pop();
    }
}
