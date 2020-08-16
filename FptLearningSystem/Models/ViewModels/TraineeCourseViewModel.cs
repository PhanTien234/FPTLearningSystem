using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FptLearningSystem.Models.ViewModels
{
    public class TraineeCourseViewModel
    {
        public TraineeCourse TraineeCourse { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public string StatusMessage { get; set; }
    }
}
