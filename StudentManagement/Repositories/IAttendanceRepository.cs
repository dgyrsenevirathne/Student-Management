using StudentManagement.Models;

namespace StudentManagement.Repositories
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAttendanceByDateAsync(DateTime date);
        Task<IEnumerable<Attendance>> GetAttendanceByStudentIdAsync(int studentId);
        Task<int> MarkAttendanceAsync(Attendance attendance);
        Task<int> UpdateAttendanceAsync(Attendance attendance);
        Task<Attendance> GetAttendanceByIdAsync(int id);
    }
}
