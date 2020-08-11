using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FptLearningSystem.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Topic Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
    }
}
