using StudentManagement.Models;
using System.Collections.Generic;

namespace StudentManagement.Repositories
{
    public interface IAssignmentRepository
    {
        Assignment GetById(int id);
        IEnumerable<Assignment> GetAll();
        int Add(Assignment assignment);
        bool Update(Assignment assignment);
        bool Delete(int id);
        IEnumerable<Course> GetAllCourses();
        IEnumerable<Student> GetAllStudents();
    }
}
