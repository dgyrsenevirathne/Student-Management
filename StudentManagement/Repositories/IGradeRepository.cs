using StudentManagement.Models;

namespace StudentManagement.Repositories
{
    public interface IGradeRepository
    {
        Task<IEnumerable<Grade>> GetAllGradesAsync();
        Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId);
        Task<Grade> GetGradeByIdAsync(int id);
        Task<int> AddGradeAsync(Grade grade);
        Task<int> UpdateGradeAsync(Grade grade);
        Task<int> DeleteGradeAsync(int id);
    }
}
