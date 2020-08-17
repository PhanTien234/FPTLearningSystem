using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FptLearningSystem.Models.ViewModels
{
    public class TrainerTopicCourseViewModel
    {
        public IEnumerable<TrainerTopic> TrainerTopic { get; set; }
        public IEnumerable<Course> CourseList { get; set; }
    }
}
