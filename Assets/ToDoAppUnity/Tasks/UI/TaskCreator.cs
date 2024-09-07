using ToDoAppUnity.Models;

namespace ToDoAppUnity.Tasks.UI
{
    public class TaskCreator
    {
        public TaskData CreateTask()
        {
            TaskData taskData = new TaskData("New Task", null, null);
            return taskData;
        }
    }
}