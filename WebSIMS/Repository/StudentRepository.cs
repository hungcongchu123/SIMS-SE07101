using Microsoft.EntityFrameworkCore;
using WebSIMS.DBContext;
using WebSIMS.DBContext.Entities;
using WebSIMS.Interfaces;

namespace WebSIMS.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SIMSDBContext _dbContext;

        public StudentRepository(SIMSDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _dbContext.StudentsDb.ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _dbContext.StudentsDb.FindAsync(id);
        }

        public async Task AddAsync(Student student)
        {
            student.EnrollmentDate ??= DateTime.Now;
            _dbContext.StudentsDb.Add(student);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            var existing = await _dbContext.StudentsDb.FindAsync(student.StudentID);
            if (existing != null)
            {
                existing.FirstName = student.FirstName;
                existing.LastName = student.LastName;
                existing.DateOfBirth = student.DateOfBirth;
                existing.Gender = student.Gender;
                existing.Email = student.Email;
                existing.Phone = student.Phone;
                existing.Address = student.Address;
                existing.Program = student.Program;
                existing.EnrollmentDate = student.EnrollmentDate;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _dbContext.StudentsDb.FindAsync(id);
            if (student != null)
            {
                _dbContext.StudentsDb.Remove(student);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
