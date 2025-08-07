// Services/Interfaces/ICourseService.cs
using WebSIMS.BDContext.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebSIMS.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Courses>> GetAllAsync();
        Task<Courses?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task AddAsync(Courses course);
        Task UpdateAsync(Courses course);
    }
}
