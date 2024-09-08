using System.Collections.Generic;
using System.Globalization;
using JorisHoef.UI;
using TMPro;
using ToDoAppUnity.Models;
using UnityEngine;
using UnityEngine.UI;

namespace ToDoAppUnity.Tasks.UI
{
    public class TaskItemDetails : MonoBehaviour
    {
        [SerializeField] private TMP_Text _createdAtText;
        [SerializeField] private TMP_Text _updatedAtText;
        [SerializeField] private RectTransform _subTasksContainer;
        [SerializeField] private TaskItem _taskItemPrefab;
        [SerializeField] private Button _addSubTaskButton;
        [SerializeField] private VisibilityController _visibilityController;
        
        private readonly TaskCreator _taskCreator = new TaskCreator();
        private TaskItemManager _parentTaskItemManager;

        private List<TaskData> _initializedSubtasks;
        
        public VisibilityController VisibilityController => _visibilityController;
        
        private TaskItem TaskItem { get; set; }
        private TaskData TaskData { get; set; }

        private void OnEnable()
        {
            _addSubTaskButton.onClick.AddListener(OnAddSubtaskClicked);
        }
        
        private void OnDisable()
        {
            _addSubTaskButton.onClick.RemoveListener(OnAddSubtaskClicked);
        }
        
        public void Initialize(TaskItem taskItem, TaskData taskData, TaskItemManager parentTaskItemManager)
        {
            this.TaskItem = taskItem;
            this.TaskData = taskData;
            this._parentTaskItemManager = parentTaskItemManager;
            SetUIData(taskData);
        }
        
        private void SetUIData(TaskData taskData)
        {
            _createdAtText.text = taskData.CreatedAt.ToString(CultureInfo.InvariantCulture);
            _updatedAtText.text = taskData.UpdatedAt.ToString(CultureInfo.InvariantCulture);

            if (taskData.SubTasks == null) return;
            _initializedSubtasks ??= new List<TaskData>();
            
            foreach (var subTaskData in taskData.SubTasks)
            {
                if(_initializedSubtasks.Contains(subTaskData)) continue;
                
                var newTaskGo = GameObject.Instantiate(_taskItemPrefab, _subTasksContainer);
                newTaskGo.Initialize(subTaskData, _parentTaskItemManager);
                _initializedSubtasks.Add(subTaskData);
            }
        }
        
        private void OnAddSubtaskClicked()
        { 
            var subTaskData = _taskCreator.CreateTask();
            subTaskData.ParentTaskId = TaskData.Id;
            TaskData.SubTasks ??= new List<TaskData>();
            TaskData.SubTasks.Add(subTaskData);
            TaskItem.UpdateTaskItem(TaskData); 
            _parentTaskItemManager.AddTaskItem(subTaskData, _subTasksContainer);
        }
    }
}