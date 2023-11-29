namespace CodeBase.Services.IServices;
public interface ICurrentUserService
{
    void Authenticate(string token);
    string GetUserId();
    bool IsAuthenticated();
    bool Logout();
}