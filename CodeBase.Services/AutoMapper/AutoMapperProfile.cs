namespace CodeBase.Services.AutoMapper;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateUserRequestDTO, ApplicationUser>()
        .ForMember(x => x.UserName, s => s.MapFrom(a => a.Email));
        CreateMap<Order, OrderDTO>()
            .ForMember(x => x.CreatedOn, a => a.MapFrom(s => s.CreatedDate.GetDateStr("dd/MM/yyyy HH:mm")))
            .ForMember(x => x.UserName, a => a.MapFrom(s => s.GetUserName()));
    CreateMap<OrderItem, OrderItemDTO>()
        .ForMember(x => x.Name, a => a.MapFrom(s => s.Book.Name));
        CreateMap<Book, BookDTO>()
            .ForMember(x => x.Created ,a => a.MapFrom(s => s.CreatedDate.GetDateStr("dd/MM/yyyy HH:mm")));
    }
}
