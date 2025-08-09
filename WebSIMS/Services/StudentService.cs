using Microsoft.EntityFrameworkCore;
using WebSIMS.DBContext;
using WebSIMS.DBContext.Entities;
using WebSIMS.Interfaces;

namespace WebSIMS.Services
{
    public class StudentService : IStudentService
    {
        private readonly SIMSDBContext _context;
        private readonly AutoEnrollmentService _autoEnrollmentService;

        public StudentService(SIMSDBContext context, AutoEnrollmentService autoEnrollmentService)
        {
            _context = context;
            _autoEnrollmentService = autoEnrollmentService;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.StudentsDb.ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.StudentsDb.FindAsync(id);
        }

        public async Task<bool> AddStudentAsync(Student student)
        {
            try
            {
                student.EnrollmentDate ??= DateTime.Now;
                _context.StudentsDb.Add(student);
                await _context.SaveChangesAsync();
                
                // Tự động đăng ký student mới vào tất cả courses hiện có
                await _autoEnrollmentService.AutoEnrollNewStudentAsync(student.StudentID);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            try
            {
                var existingStudent = await _context.StudentsDb.FindAsync(student.StudentID);
                if (existingStudent == null)
                    return false;

                existingStudent.StudentCode = student.StudentCode;
                existingStudent.FirstName = student.FirstName;
                existingStudent.LastName = student.LastName;
                existingStudent.DateOfBirth = student.DateOfBirth;
                existingStudent.Gender = student.Gender;
                existingStudent.Email = student.Email;
                existingStudent.Phone = student.Phone;
                existingStudent.Address = student.Address;
                existingStudent.Program = student.Program;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            try
            {
                var student = await _context.StudentsDb.FindAsync(id);
                if (student == null)
                    return false;

                _context.StudentsDb.Remove(student);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsStudentCodeExistsAsync(string studentCode, int excludeStudentId = 0)
        {
            return await _context.StudentsDb
                .AnyAsync(s => s.StudentCode == studentCode && s.StudentID != excludeStudentId);
        }
    }
}
