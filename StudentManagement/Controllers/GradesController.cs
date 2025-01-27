using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Controllers
{
    public class GradesController : Controller
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly IStudentRepository _studentRepository;

        public GradesController(IGradeRepository gradeRepository, IStudentRepository studentRepository)
        {
            _gradeRepository = gradeRepository;
            _studentRepository = studentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var grades = await _gradeRepository.GetAllGradesAsync();
            return View(grades);
        }

        public async Task<IActionResult> StudentGrades(int id)
        {
            var grades = await _gradeRepository.GetGradesByStudentIdAsync(id);
            ViewBag.StudentId = id; // Add this line to handle empty grades case

            // If no grades found, we still want to show the page with "No grades" message
            if (!grades.Any())
            {
                var student = await _studentRepository.GetStudentByIdAsync(id);
                if (student == null) return NotFound();
                ViewBag.StudentName = student.Name;
            }

            return View(grades);
        }

        public async Task<IActionResult> Create(int studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null) return NotFound();

            ViewBag.StudentId = studentId;
            ViewBag.StudentName = student.Name;

            // Get courses for dropdown
            var courses = await _studentRepository.GetAllCoursesAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Name");

            return View(new Grade { StudentId = studentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Grade grade)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Add logging to debug
                    Console.WriteLine($"Creating grade: StudentId={grade.StudentId}, CourseId={grade.CourseId}, Grade={grade.GradeValue}");

                    var result = await _gradeRepository.AddGradeAsync(grade);

                    if (result > 0)
                    {
                        return RedirectToAction(nameof(StudentGrades), new { id = grade.StudentId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to add grade");
                    }
                }
                else
                {
                    // Log validation errors
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine($"Validation error: {error.ErrorMessage}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error creating grade: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while saving the grade.");
            }

            // If we get here, something went wrong
            var student = await _studentRepository.GetStudentByIdAsync(grade.StudentId);
            ViewBag.StudentId = grade.StudentId;
            ViewBag.StudentName = student?.Name;
            ViewBag.Courses = new SelectList(await _studentRepository.GetAllCoursesAsync(), "Id", "Name", grade.CourseId);

            return View(grade);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var grade = await _gradeRepository.GetGradeByIdAsync(id);
            if (grade == null) return NotFound();

            ViewBag.StudentName = grade.StudentName;
            ViewBag.CourseName = grade.CourseName;
            return View(grade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Grade grade)
        {
            if (id != grade.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                await _gradeRepository.UpdateGradeAsync(grade);
                return RedirectToAction("StudentGrades", new { id = grade.StudentId });
            }
            return View(grade);
        }

        // Add these methods after the Edit action
        public async Task<IActionResult> Delete(int id)
        {
            var grade = await _gradeRepository.GetGradeByIdAsync(id);
            if (grade == null) return NotFound();

            ViewBag.StudentName = grade.StudentName;
            ViewBag.CourseName = grade.CourseName;
            return View(grade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grade = await _gradeRepository.GetGradeByIdAsync(id);
            if (grade == null) return NotFound();

            await _gradeRepository.DeleteGradeAsync(id);
            return RedirectToAction(nameof(StudentGrades), new { id = grade.StudentId });
        }
    }
}
