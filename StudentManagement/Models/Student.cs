using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required]
        [StringLength(20)]
        public string Contact { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string EnrollmentNumber { get; set; }

        [Required]
        public int CourseId { get; set; }

        [NotMapped]  // This indicates the property isn't stored in the database
        [Display(Name = "Course Name")]
        public string? CourseName { get; set; }  // Make nullable

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        public bool Status { get; set; } = true;

        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }
    }
}
