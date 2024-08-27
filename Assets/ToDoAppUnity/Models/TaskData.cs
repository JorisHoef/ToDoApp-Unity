using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ToDoAppUnity.Tasks;

namespace ToDoAppUnity.Models
{
    /// <summary>
    /// Datamodel object representing a Task
    /// </summary>
    public class TaskData
    {
        public TaskData(string name, TaskDataMessage taskDataMessage, ICollection<TaskData> subTasks)
        {
            this.Name = name;
            this.TaskDataMessage = taskDataMessage;
            this.SubTasks = subTasks;
        }
        
        public long Id { get; set; }
        public string? Name { get; set; }
        public TaskDataMessage? TaskDataMessage { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TaskDataState TaskDataState { get; set; } = TaskDataState.OPEN;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CompletedAt { get; set; }
        public DateTime DeadlineAt { get; set; }
        public ICollection<TaskData>? SubTasks { get; set; } = null;
    }
}