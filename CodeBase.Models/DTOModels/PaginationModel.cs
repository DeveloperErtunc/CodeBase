namespace CodeBase.Models.DTOModels;
public class PaginationModel<T> where T : class
{
    public List<T> Datas { get; set; }
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }

    public PaginationModel(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        Datas =items;
    }
}
