using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FptLearningSystem.Models
{
    public class TrainerTopic
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Trainer")]
        public string TrainerId { get; set; }
        [ForeignKey("TrainerId")]
        public virtual ApplicationUser User { get; set; }
        [Required]
        [Display(Name = "Topic")]
        public int TopicId { get; set; }
        [ForeignKey("TopicId")]
        public virtual Topic Topic { get; set; }
    }
}
