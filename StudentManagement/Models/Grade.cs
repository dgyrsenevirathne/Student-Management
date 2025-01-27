using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class Grade
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Student ID is required")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Course ID is required")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Grade value is required")]
        [StringLength(40)]
        [Display(Name = "Grade")]
        public string GradeValue { get; set; }

        [NotMapped]
        [Display(Name = "Student Name")]
        public string? StudentName { get; set; }  // Make nullable

        [NotMapped]
        [Display(Name = "Course Name")]
        public string? CourseName { get; set; }   // Make nullable
    }
}
