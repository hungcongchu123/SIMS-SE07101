using WebSIMS.DBContext.Entities;

namespace WebSIMS.Interfaces
{
    public interface IUserRepository
    {
        Task<Users?> GetUserByUsername(string username);
        Task<Users?> GetUserById(int id);
        Task AddAsync (Users user);
        Task SaveChangeAsync();
    }
}
