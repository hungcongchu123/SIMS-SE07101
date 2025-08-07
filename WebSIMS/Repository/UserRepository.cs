using Microsoft.EntityFrameworkCore;
using WebSIMS.DBContext.Entities;
using WebSIMS.BDContext;
using WebSIMS.Interfaces;

namespace WebSIMS.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SIMSDBContext _dbContext;
        public UserRepository(SIMSDBContext context)
        {
            _dbContext = context;
        }
        public async Task AddAsync(Users user)
        {
            await _dbContext.UsersDb.AddAsync(user);
        }

        public async Task<Users?> GetUserById(int id)
        {
            return await _dbContext.UsersDb.FindAsync(id).AsTask();
        }

        public async Task<Users?> GetUserByUsername(string username)
        {
            return await _dbContext.UsersDb.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task SaveChangeAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
