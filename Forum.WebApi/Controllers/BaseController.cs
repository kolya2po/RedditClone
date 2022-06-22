using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebApi.Controllers
{
    /// <summary>
    /// Base controller, extends ControllerBase class.
    /// </summary>
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Shared Mapper object.
        /// </summary>
        protected readonly IMapper Mapper;

        /// <summary>
        /// Initializes new instance of the BaseController controller and
        /// sets value to the Mapper field.
        /// </summary>
        /// <param name="mapper">Instance of Mapper class.</param>
        protected BaseController(IMapper mapper)
        {
            Mapper = mapper;
        }
    }
}
