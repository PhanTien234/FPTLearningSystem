using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FptLearningSystem.Models
{
    public class TraineeCourse
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Trainee")]
        public string TraineeId { get; set; }
        [ForeignKey("TraineeId")]
        public virtual ApplicationUser User { get; set; }
        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
    }
}
