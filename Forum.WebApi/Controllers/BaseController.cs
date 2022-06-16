using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IMapper Mapper;

        protected BaseController(IMapper mapper)
        {
            Mapper = mapper;
        }
    }
}
