using System.Threading.Tasks;
using Business.MapModels;
using Business.MapModels.Identity;

namespace Business.Interfaces
{
    /// <summary>
    /// Describes methods for the user's authentification.
    /// Also describes Get and Update operations with user.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Returns asynchronously, mapped from the entity with specified id, model of type UserModel.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <returns>Task that encapsulates UserModel object.</returns>
        Task<UserModel> GetByIdAsync(int userId);

        /// <summary>
        /// Updates asynchronously mapped object of type UserModel in the table.
        /// </summary>
        /// <param name="model">UserModel to be updated.</param>
        /// <returns>Task.</returns>
        Task UpdateAsync(UserModel model);

        /// <summary>
        /// Creates asynchronously new user in the table. Then adds new claim to the user
        /// and generates JWT token, based on the user's claims.
        /// </summary>
        /// <param name="model">RegistrationModel's object.</param>
        /// <returns>UserDto's object which has user's id and token.</returns>
        Task<UserDto> RegistrationAsync(RegistrationModel model);

        /// <summary>
        /// Asynchronously logins  user. Then gets user's claims and generates JWT token,
        /// based on those claims.
        /// </summary>
        /// <param name="model">LoginModel's object.</param>
        /// <returns>UserDto's object which has user's id and token.</returns>
        Task<UserDto> LoginAsync(LoginModel model);

        /// <summary>
        /// Asynchronously signs out user.
        /// </summary>
        /// <returns>Task.</returns>
        Task LogoutAsync();
    }
}
