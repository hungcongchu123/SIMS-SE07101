using WebSIMS.DBContext.Entities;
using WebSIMS.Interfaces;

namespace WebSIMS.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _studentRepository.GetAllAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _studentRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Student student)
        {
            await _studentRepository.AddAsync(student); // SaveChangesAsync() đã có trong repo
        }

        public async Task UpdateAsync(Student student)
        {
            await _studentRepository.UpdateAsync(student); // repo đã SaveChangesAsync()
        }

        public async Task DeleteAsync(int id)
        {
            await _studentRepository.DeleteAsync(id); // repo đã SaveChangesAsync()
        }
    }
}
