using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskSystem.Server.Models
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }

        public int? PriorityId { get; set; }
        public string PriorityName { get; set; }

        public int UserId { get; set; }
    }
}