using CA.DAL.Entity;
using CA.DTO.Models;
using System.Threading.Tasks;

namespace CA.BLL.Services
{
    public interface IUserService
    {
        Task<int> RegisterEmail(RegisterEmailModel model);
        Task<bool> VerifyEmail(VerifyModel model);
        Task<User> CheckPersonById(int id);
    }
}