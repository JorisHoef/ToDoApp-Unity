using System.Collections.Generic;

namespace ToDoAppUnity.Models
{
    public class TaskItemMessage
    {
        public long Id { get; set; }
        public string? Message { get; set; }
        public IList<long>? ReferencedTaskIds { get; set; }
    }
}