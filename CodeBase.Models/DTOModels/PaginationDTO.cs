namespace CodeBase.Models.DTOModels;
public class PaginationDTO
{
    [Range(1,int.MaxValue)]
    public int PageIndex { get;  set; }
    [Range(1, int.MaxValue)]
    public int PageSize { get;  set; }

    public DateTime? StatDate { get; set; }
    public DateTime? EndDate  { get; set; }
    public string?  UserId { get; set; }
}
