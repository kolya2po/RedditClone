using System.Threading.Tasks;
using Business.MapModels;
using Business.MapModels.Identity;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> GetByIdAsync(int userId);
        Task UpdateAsync(UserModel model);
        Task<bool> RegistrationAsync(RegistrationModel model);
        Task<bool> LoginAsync(LoginModel model);
        Task LogoutAsync();
    }
}
