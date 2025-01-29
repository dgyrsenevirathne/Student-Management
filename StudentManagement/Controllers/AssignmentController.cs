using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public AssignmentController(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public IActionResult Index()
        {
            var assignments = _assignmentRepository.GetAll();
            return View(assignments);
        }

        public IActionResult Create()
        {
            ViewBag.Courses = _assignmentRepository.GetAllCourses();
            ViewBag.Students = _assignmentRepository.GetAllStudents();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Assignment assignment)
        {
            Console.WriteLine($"Received: {assignment.Title} | {assignment.DueDate} | {assignment.CourseId} | {assignment.StudentId}");
            if (ModelState.IsValid)
            {
                var newId = _assignmentRepository.Add(assignment);
                Console.WriteLine($"New assignment ID: {newId}");
                return RedirectToAction(nameof(Index));
            }
            // Log model state errors
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Validation error: {error.ErrorMessage}");
            }
            ViewBag.Courses = _assignmentRepository.GetAllCourses();
            ViewBag.Students = _assignmentRepository.GetAllStudents();
            return View(assignment);
        }

        public IActionResult Edit(int id)
        {
            var assignment = _assignmentRepository.GetById(id);
            if (assignment == null)
                return NotFound();

            ViewBag.Courses = _assignmentRepository.GetAllCourses();
            ViewBag.Students = _assignmentRepository.GetAllStudents();
            return View(assignment);
        }

        [HttpPost]
        public IActionResult Edit(Assignment assignment)
        {
            if (ModelState.IsValid && _assignmentRepository.Update(assignment))
                return RedirectToAction(nameof(Index));

            ViewBag.Courses = _assignmentRepository.GetAllCourses();
            ViewBag.Students = _assignmentRepository.GetAllStudents();
            return View(assignment);
        }

        public IActionResult Delete(int id)
        {
            var assignment = _assignmentRepository.GetById(id);
            if (assignment == null)
                return NotFound();

            return View(assignment);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _assignmentRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
