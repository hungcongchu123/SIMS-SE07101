using WebSIMS.DBContext.Entities;
using WebSIMS.Services.Interfaces;
using WebSIMS.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSIMS.Services
{
    public class FacultyService : IFacultyService
    {
        private readonly SIMSDBContext _context;

        public FacultyService(SIMSDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Faculty>> GetAllAsync()
        {
            return await _context.Faculties.Include(f => f.User).ToListAsync();
        }

        public async Task<Faculty?> GetByIdAsync(int id)
        {
            return await _context.Faculties.Include(f => f.User).FirstOrDefaultAsync(f => f.FacultyID == id);
        }

        public async Task AddAsync(Faculty faculty)
        {
            _context.Faculties.Add(faculty);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Faculty faculty)
        {
            _context.Faculties.Update(faculty);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty != null)
            {
                _context.Faculties.Remove(faculty);
                await _context.SaveChangesAsync();
            }
        }
    }
}