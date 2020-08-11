using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FptLearningSystem.Models.ViewModels
{
    public class TopicAndCourseViewModel
    {
        public IEnumerable<Course> CourseList { get; set; }
        public Topic Topic { get; set; }
        public List<string> TopicList { get; set; }
        public string StatusMessage { get; set; }
    }
}
