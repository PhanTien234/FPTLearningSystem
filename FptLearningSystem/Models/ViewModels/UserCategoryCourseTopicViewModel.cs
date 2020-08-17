using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FptLearningSystem.Models.ViewModels
{
    public class UserCategoryCourseTopicViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<Topic> Topics { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Course> Courses { get; set; }
    }
}
