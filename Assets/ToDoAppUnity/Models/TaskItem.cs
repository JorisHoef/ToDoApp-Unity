using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ToDoAppUnity.Models
{
    public static class TaskItemManager
    {
        public static List<TaskItem> AllTaskItems { get; set; } = new List<TaskItem>();
    }
    
    public class TaskItem
    {
        public static TaskItem CreatePostResource()
        {
            return new TaskItem("TestName", new TaskItemMessage(), new List<TaskItem>());
        }

        public static TaskItem AddNewTaskItemMessage()
        {
            const int idToGet = 1;
            
            TaskItem taskItem = TaskItemManager.AllTaskItems.FirstOrDefault(x => x.Id == idToGet);
            
            var taskItemMessage = new TaskItemMessage
            {
                    Message = $"Wow, this is such a cool taskMessage and I'm taking into account an existing Task look: {taskItem?.Id}", 
                    ReferencedTaskIds = new List<long>(){idToGet},
            };
            
            return new TaskItem("TestName", taskItemMessage, new List<TaskItem>());
        }

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