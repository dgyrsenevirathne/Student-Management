using Dapper;
using Microsoft.Data.SqlClient;
using StudentManagement.Models;

namespace StudentManagement.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly string _connectionString;

        public AttendanceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByDateAsync(DateTime date)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
            SELECT a.*, s.Name as StudentName 
            FROM Attendance a
            JOIN Students s ON a.StudentId = s.Id
            WHERE CAST(a.Date as DATE) = CAST(@Date as DATE)";
            return await connection.QueryAsync<Attendance>(query, new { Date = date });
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByStudentIdAsync(int studentId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
            SELECT a.*, s.Name as StudentName 
            FROM Attendance a
            JOIN Students s ON a.StudentId = s.Id
            WHERE a.StudentId = @StudentId
            ORDER BY a.Date DESC";
            return await connection.QueryAsync<Attendance>(query, new { StudentId = studentId });
        }

        public async Task<int> MarkAttendanceAsync(Attendance attendance)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
            INSERT INTO Attendance (StudentId, Date, Status)
            VALUES (@StudentId, @Date, @Status);
            SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.QuerySingleAsync<int>(query, attendance);
        }

        public async Task<int> UpdateAttendanceAsync(Attendance attendance)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
            UPDATE Attendance 
            SET Status = @Status
            WHERE Id = @Id";
            return await connection.ExecuteAsync(query, attendance);
        }

        public async Task<Attendance> GetAttendanceByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
            SELECT a.*, s.Name as StudentName 
            FROM Attendance a
            JOIN Students s ON a.StudentId = s.Id
            WHERE a.Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Attendance>(query, new { Id = id });
        }
    }
}
