using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _repository;

        public StudentsController(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _repository.GetAllStudentsAsync();
            return View(students);
        }

        public async Task<IActionResult> Details(int id)
        {
            var student = await _repository.GetStudentByIdAsync(id);
            if (student == null) return NotFound();
            return View(student);
        }

        public async Task<IActionResult> Create()
        {
            var courses = await _repository.GetAllCoursesAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            try
            {
                Console.WriteLine($"Attempting to create student: {student.Name}, Email: {student.Email}");

                if (ModelState.IsValid)
                {
                    // Create a new student object with only the database fields
                    var newStudent = new Student
                    {
                        Name = student.Name,
                        DOB = student.DOB,
                        Contact = student.Contact,
                        Email = student.Email,
                        EnrollmentNumber = student.EnrollmentNumber,
                        CourseId = student.CourseId
                    };

                    var result = await _repository.AddStudentAsync(newStudent);
                    Console.WriteLine($"Student created with ID: {result}");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating student: {ex.Message}");
                ModelState.AddModelError("", $"Failed to create student: {ex.Message}");
            }

            // If we get here, something went wrong - reload the courses for the dropdown
            var courses = await _repository.GetAllCoursesAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Name");
            return View(student);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var student = await _repository.GetStudentByIdAsync(id);
            if (student == null) return NotFound();

            var courses = await _repository.GetAllCoursesAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Name", student.CourseId);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                await _repository.UpdateStudentAsync(student);
                return RedirectToAction(nameof(Index));
            }
            var courses = await _repository.GetAllCoursesAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Name", student.CourseId);
            return View(student);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var student = await _repository.GetStudentByIdAsync(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection form)
        {
            await _repository.DeleteStudentAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
