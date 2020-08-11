using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FptLearningSystem.Models.ViewModels
{
    public class CourseAndCategoryViewModel
    {
        public IEnumerable<Category> CaterotyList { get; set; }
        public Course Course { get; set; }
        public List<string> CourseList { get; set; }
        public string StatusMessage { get; set; }
    }
}
