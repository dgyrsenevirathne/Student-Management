using Dapper;
using Microsoft.Data.SqlClient;
using StudentManagement.Models;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace StudentManagement.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
            SELECT s.*, c.Name AS CourseName
            FROM Students s
            LEFT JOIN Courses c ON s.CourseId = c.Id
            WHERE s.Status = 1";
            return await connection.QueryAsync<Student>(query);
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
            SELECT s.*, c.Name AS CourseName
            FROM Students s
            LEFT JOIN Courses c ON s.CourseId = c.Id
            WHERE s.Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Student>(query, new { Id = id });
        }

        public async Task<int> AddStudentAsync(Student student)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var sql = @"
            INSERT INTO Students (Name, DOB, Contact, Email, EnrollmentNumber, CourseId, CreatedAt, Status)
            VALUES (@Name, @DOB, @Contact, @Email, @EnrollmentNumber, @CourseId, GETDATE(), 1);
            SELECT CAST(SCOPE_IDENTITY() as int)";

                var parameters = new
                {
                    student.Name,
                    student.DOB,
                    student.Contact,
                    student.Email,
                    student.EnrollmentNumber,
                    student.CourseId
                };

                // Debug logging
                Console.WriteLine($"Executing SQL with parameters:");
                Console.WriteLine($"Name: {student.Name}");
                Console.WriteLine($"DOB: {student.DOB}");
                Console.WriteLine($"Contact: {student.Contact}");
                Console.WriteLine($"Email: {student.Email}");
                Console.WriteLine($"EnrollmentNumber: {student.EnrollmentNumber}");
                Console.WriteLine($"CourseId: {student.CourseId}");

                var result = await connection.QuerySingleOrDefaultAsync<int>(sql, parameters);
                Console.WriteLine($"Query result: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw; // Re-throw the exception to be handled by the controller
            }
        }

        public async Task<int> UpdateStudentAsync(Student student)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
            UPDATE Students
            SET Name = @Name, DOB = @DOB, Contact = @Contact, 
                EnrollmentNumber = @EnrollmentNumber, Email = @Email, CourseId = @CourseId
            WHERE Id = @Id";
            return await connection.ExecuteAsync(query, student);
        }

        public async Task<int> DeleteStudentAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
            UPDATE Students 
            SET Status = 0, 
            DeletedAt = GETDATE() 
            WHERE Id = @Id";
            return await connection.ExecuteAsync(query, new { Id = id });
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Courses";
            return await connection.QueryAsync<Course>(query);
        }

    }
}
