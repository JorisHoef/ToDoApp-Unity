using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ToDoAppUnity.Tasks;

namespace ToDoAppUnity.Models
{
    public class TaskItem
    {
        public TaskItem(string name, TaskItemMessage taskItemMessage, ICollection<TaskItem> subTasks)
        {
            this.Name = name;
            this.TaskItemMessage = taskItemMessage;
            this.SubTasks = subTasks;
        }
        
        public long Id { get; set; }
        public string? Name { get; set; }
        public TaskItemMessage? TaskItemMessage { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TaskItemState TaskItemState { get; set; } = TaskItemState.OPEN;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<TaskItem>? SubTasks { get; set; } = null;
    }
}