namespace CodeBase.Models.DBModels;
public class ApplicationUser:IdentityUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public virtual List<Order>? Orders { get; set; }
    public  virtual List<ApplicationUserRole>? UserRoles { get; set; }
}
public class ApplicationRole : IdentityRole
{
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}

public class ApplicationUserRole : IdentityUserRole<string>
{
    public virtual ApplicationUser User { get; set; }
    public virtual ApplicationRole Role { get; set; }
}