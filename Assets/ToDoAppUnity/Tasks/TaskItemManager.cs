using System.Collections.Generic;
using System.Linq;
using ToDoAppUnity.Models;

namespace ToDoAppUnity.Tasks
{
    public static class TaskItemManager
    {
        public static List<TaskItem> AllTaskItems { get; set; } = new List<TaskItem>();
        
        public static TaskItem CreatePostResource()
        {
            return new TaskItem("TestName", new TaskItemMessage(), new List<TaskItem>());
        }
        
        public static TaskItem AddNewTaskItemMessage()
        {
            const int idToGet = 1;
            
            TaskItem taskItem = AllTaskItems.FirstOrDefault(x => x.Id == idToGet);
            
            var taskItemMessage = new TaskItemMessage
            {
                    Message = $"Wow, this is such a cool taskMessage and I'm taking into account an existing Task look: {{TASK_ID:{taskItem?.Id}}}", 
                    ReferencedTaskIds = new List<long>(){idToGet},
            };
            
            return new TaskItem("TestName", taskItemMessage, new List<TaskItem>());
        }
    }
}