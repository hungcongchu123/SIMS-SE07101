using Microsoft.EntityFrameworkCore;
using WebSIMS.DBContext;
using WebSIMS.DBContext.Entities;
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

        public void Update(Users user)
        {
            _dbContext.UsersDb.Update(user);
        }

        public void Delete(Users user)
        {
            _dbContext.UsersDb.Remove(user);
        }

        public async Task<Users?> GetUserById(int id)
        {
            return await _dbContext.UsersDb.FindAsync(id);
        }

        public async Task<Users?> GetUserByUsername(string username)
        {
            return await _dbContext.UsersDb.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<List<Users>> GetAllAsync()
        {
            return await _dbContext.UsersDb.ToListAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public Task<List<Users>> GetUsersByRoleAsync(string role)
        {
            throw new NotImplementedException();
        }
    }
}