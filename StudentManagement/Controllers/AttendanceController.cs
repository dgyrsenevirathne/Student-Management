using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IStudentRepository _studentRepository;

        public AttendanceController(IAttendanceRepository attendanceRepository, IStudentRepository studentRepository)
        {
            _attendanceRepository = attendanceRepository;
            _studentRepository = studentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var attendance = await _attendanceRepository.GetAttendanceByDateAsync(today);
            ViewBag.Date = today;
            return View(attendance);
        }

        public async Task<IActionResult> MarkAttendance()
        {
            var students = await _studentRepository.GetAllStudentsAsync();
            ViewBag.Date = DateTime.Today;
            return View(students);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAttendance(Dictionary<int, bool> studentAttendance, DateTime date)
        {
            foreach (var attendance in studentAttendance)
            {
                await _attendanceRepository.MarkAttendanceAsync(new Attendance
                {
                    StudentId = attendance.Key,
                    Date = date,
                    Status = attendance.Value
                });
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> StudentAttendance(int id)
        {
            var attendance = await _attendanceRepository.GetAttendanceByStudentIdAsync(id);
            var student = await _studentRepository.GetStudentByIdAsync(id);
            ViewBag.StudentName = student.Name;
            return View(attendance);
        }
    }
}
