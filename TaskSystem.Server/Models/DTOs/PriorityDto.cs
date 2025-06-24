using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskSystem.Server.Models.DTOs
{
    public class PriorityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}