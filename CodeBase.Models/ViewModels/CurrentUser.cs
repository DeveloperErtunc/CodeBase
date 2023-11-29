namespace CodeBase.Models.ViewModels;
public class CurrentUser
{
    public CurrentUser(string userId, string userMail, string fullName, List<string> accountRoles)
    {
        UserId = userId;
        FullName = fullName;
        UserMail = userMail;
        AccountRoles = accountRoles;
    }
    public string UserId { get; set; }
    public string UserMail { get; set; }
    public string FullName { get; set; }
    public List<string> AccountRoles { get; set; }
}