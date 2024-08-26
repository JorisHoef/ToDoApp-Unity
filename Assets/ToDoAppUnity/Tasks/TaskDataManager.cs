using System.Collections.Generic;
using System.Linq;
using ToDoAppUnity.Models;

namespace ToDoAppUnity.Tasks
{
    public static class TaskDataManager
    {
        public static List<TaskData> AllTaskItems { get; set; } = new List<TaskData>();
        
        public static TaskData CreatePostResource()
        {
            return new TaskData("TestName", new TaskDataMessage(), new List<TaskData>());
        }
        
        public static TaskData AddNewTaskItemMessage()
        {
            const int idToGet = 1;
            
            TaskData taskItem = AllTaskItems.FirstOrDefault(x => x.Id == idToGet);
            
            var taskItemMessage = new TaskDataMessage
            {
                    Message = $"Wow, this is such a cool taskMessage and I'm taking into account an existing Task look: {{TASK_ID:{taskItem?.Id}}}", 
                    ReferencedTaskIds = new List<long>(){idToGet},
            };
            
            return new TaskData("TestName", taskItemMessage, new List<TaskData>());
        }
    }
}