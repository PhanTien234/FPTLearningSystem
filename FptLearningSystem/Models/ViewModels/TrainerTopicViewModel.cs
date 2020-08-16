using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FptLearningSystem.Models.ViewModels
{
    public class TrainerTopicViewModel
    {
        public TrainerTopic TrainerTopic { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<Topic> Topics { get; set; }
        public string StatusMessage { get; set; }
    }
}
