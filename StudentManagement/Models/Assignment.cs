using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class Assignment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Due Date is required")]
        [Display(Name = "Due Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Student is required")]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [NotMapped]
        public string? CourseName { get; set; } 

        [NotMapped]
        public string? StudentName { get; set; } 
    }
}
