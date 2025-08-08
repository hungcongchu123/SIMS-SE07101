using WebSIMS.DBContext.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSIMS.Services.Interfaces
{
    public interface IFacultyService
    {
        Task<IEnumerable<Faculty>> GetAllAsync();
        Task<Faculty?> GetByIdAsync(int id);
        Task AddAsync(Faculty faculty);
        Task UpdateAsync(Faculty faculty);
        Task DeleteAsync(int id);
    }
}