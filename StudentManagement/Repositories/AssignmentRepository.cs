using Dapper;
using Microsoft.Data.SqlClient;
using StudentManagement.Models;
using System.Data;
using System.Collections.Generic;

namespace StudentManagement.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly string _connectionString;

        public AssignmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Assignment> GetAll()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = @"SELECT a.*, c.Name AS CourseName, s.Name AS StudentName 
                      FROM Assignments a
                      INNER JOIN Courses c ON a.CourseId = c.Id
                      INNER JOIN Students s ON a.StudentId = s.Id";
            return db.Query<Assignment>(sql);
        }

        public Assignment GetById(int id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.QueryFirstOrDefault<Assignment>("SELECT * FROM Assignments WHERE Id = @Id", new { Id = id });
        }

        public int Add(Assignment assignment)
        {
            try
            {
                using IDbConnection db = new SqlConnection(_connectionString);
                var sql = @"INSERT INTO Assignments (Title, Description, DueDate, CourseId, StudentId)
                      VALUES (@Title, @Description, @DueDate, @CourseId, @StudentId);
                      SELECT CAST(SCOPE_IDENTITY() as int)";
                return db.QuerySingle<int>(sql, assignment);
            }
            catch (Exception ex)
            {
                // Log the error (implement logging)
                Console.WriteLine($"Error saving assignment: {ex.Message}");
                return -1;
            }
            
        }

        public bool Update(Assignment assignment)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = @"UPDATE Assignments SET 
                      Title = @Title,
                      Description = @Description,
                      DueDate = @DueDate,
                      CourseId = @CourseId,
                      StudentId = @StudentId
                      WHERE Id = @Id";
            return db.Execute(sql, assignment) > 0;
        }

        public bool Delete(int id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.Execute("DELETE FROM Assignments WHERE Id = @Id", new { Id = id }) > 0;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.Query<Course>("SELECT * FROM Courses");
        }

        public IEnumerable<Student> GetAllStudents()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.Query<Student>("SELECT * FROM Students");
        }
    }
}
