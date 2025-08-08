// File: ICourseService.cs
using WebSIMS.BDContext.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebSIMS.Services.Interfaces // Namespace chính xác
{
    public interface ICourseService // Tên interface chính xác
    {
        Task<IEnumerable<Courses>> GetAllAsync();
        Task<Courses?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task AddAsync(Courses course);
        Task UpdateAsync(Courses course);
    }
}