using AutoMapper;
using Data.Interfaces;

namespace Business.Services
{
    /// <summary>
    /// Base service. Contains shared fields.
    /// </summary>
    public class BaseService
    {
        /// <summary>
        /// Field that stores instance that implements IUnitOfWork interface.
        /// </summary>
        protected readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Field that stores instance that implements IMapper interface.
        /// </summary>
        protected readonly IMapper Mapper;

        /// <summary>
        /// Initializes a new instance of the BaseRepository.
        /// Also initializes class's fields.
        /// </summary>
        /// <param name="unitOfWork">Instance of the UnitOfWork class.</param>
        /// <param name="mapper">Instance of the Mapper class.</param>
        protected BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }
    }
}
