namespace CodeBase.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    readonly IMapper _mapper;
    public BaseController(IMapper mapper) => _mapper = mapper;
    [NonAction]
    public string ChekModelState(ModelStateDictionary state)
    {
        if (!state.IsValid)
             return string.Join(",", state.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));

        return string.Empty;
    }
}
