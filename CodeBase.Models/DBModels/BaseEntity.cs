namespace CodeBase.Models.DBModels;
public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedDate { get; private set; }
    public DateTimeOffset UpdateDate { get; private set; }
    public string CreatedBy { get; private set; }
    public string? UpdatedBy { get; private set; }
    public bool IsDeleted { get; private set; }
    public virtual void Created(string userid, Guid? id = null)
    {
        UpdateDate = CreatedDate = DateTimeOffset.UtcNow;
        CreatedBy = userid;
        Id = id ?? Guid.Empty;
    }
    public virtual void Updated(string userid)
    {
        UpdateDate = DateTimeOffset.UtcNow;
        UpdatedBy = userid;
    }
    public virtual void Deleted(string userid)
    {
        UpdateDate = DateTimeOffset.UtcNow;
        IsDeleted = true;
    }
}
public interface IBaseEntity
{
    Guid Id { get; set; }
    DateTimeOffset CreatedDate { get; }
    DateTimeOffset UpdateDate { get; }
    string CreatedBy { get; }
    string? UpdatedBy { get; }
    bool IsDeleted { get; }
    void Created(string userid, Guid? id = null);
    void Updated(string userid);
    void Deleted(string userid);
}
