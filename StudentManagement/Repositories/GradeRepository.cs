using Dapper;
using Microsoft.Data.SqlClient;
using StudentManagement.Models;

namespace StudentManagement.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly string _connectionString;

        public GradeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Grade>> GetAllGradesAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
            SELECT g.*, s.Name as StudentName, c.Name as CourseName
            FROM Grades g
            JOIN Students s ON g.StudentId = s.Id
            JOIN Courses c ON g.CourseId = c.Id";
            return await connection.QueryAsync<Grade>(query);
        }

        public async Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
            SELECT 
            g.Id,
            g.StudentId,
            g.CourseId,
            g.Grade as GradeValue,  -- Map the Grade column to GradeValue property
            s.Name as StudentName,
            c.Name as CourseName
            FROM Grades g
            JOIN Students s ON g.StudentId = s.Id
            JOIN Courses c ON g.CourseId = c.Id
            WHERE g.StudentId = @StudentId";

            return await connection.QueryAsync<Grade>(query, new { StudentId = studentId });
        }

        public async Task<Grade> GetGradeByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
            SELECT 
            g.Id,
            g.StudentId,
            g.CourseId,
            g.Grade as GradeValue,  -- Map the Grade column to GradeValue property
            s.Name as StudentName, 
            c.Name as CourseName
            FROM Grades g
            INNER JOIN Students s ON g.StudentId = s.Id
            INNER JOIN Courses c ON g.CourseId = c.Id
            WHERE g.Id = @Id";

            return await connection.QuerySingleOrDefaultAsync<Grade>(query, new { Id = id });
        }

        public async Task<int> AddGradeAsync(Grade grade)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
            INSERT INTO Grades (StudentId, CourseId, Grade)  -- Note: 'Grade' is the column name
            VALUES (@StudentId, @CourseId, @Grade);
            SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new
            {
                StudentId = grade.StudentId,
                CourseId = grade.CourseId,
                Grade = grade.GradeValue  // Map GradeValue to Grade column
            };

            return await connection.QuerySingleAsync<int>(query, parameters);
        }

        public async Task<int> UpdateGradeAsync(Grade grade)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
            UPDATE Grades 
            SET Grade = @GradeValue
            WHERE Id = @Id";
            return await connection.ExecuteAsync(query, grade);
        }

        public async Task<int> DeleteGradeAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Grades WHERE Id = @Id";
            return await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
