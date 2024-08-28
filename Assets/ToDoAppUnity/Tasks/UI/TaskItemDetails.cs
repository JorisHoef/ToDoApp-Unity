using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using ToDoAppUnity.Models;
using UnityEngine;

namespace ToDoAppUnity.Tasks.UI
{
    public class TaskItemDetails : MonoBehaviour
    {
        [SerializeField] private TMP_Text _createdAtText;
        [SerializeField] private TMP_Text _updatedAtText;
        [SerializeField] private RectTransform _subTasksContainer;
        [SerializeField] private TaskItem _taskItemPrefab;
        
        private TaskItemManager _parentTaskItemManager;
        private List<TaskData> _subTasks = new List<TaskData>();
        
        private TaskData TaskData { get; set; }

        public void Initialize(TaskData taskData, TaskItemManager parentTaskItemManager)
        {
            this.TaskData = taskData;
            this._parentTaskItemManager = parentTaskItemManager;
            SetUIData(taskData);
        }

        private static int _subTaskCounter; //Hardcoded safety for testing and circumvent infinite recursion when adding subtask into subtask
        
        private void SetUIData(TaskData taskData)
        {
            _createdAtText.text = taskData.CreatedAt.ToString(CultureInfo.InvariantCulture);
            _updatedAtText.text = taskData.UpdatedAt.ToString(CultureInfo.InvariantCulture);

            if (_subTaskCounter == 1) return;
            _subTaskCounter = 1;
            
            //Just add itself for now to test
            if (!_subTasks.Contains(taskData))
            {
                _subTasks.Add(taskData);
                foreach (var subTaskData in _subTasks)
                {
                    var newTaskGo = GameObject.Instantiate(_taskItemPrefab, _subTasksContainer);
                    newTaskGo.Initialize(subTaskData, _parentTaskItemManager);
                }
            }
        }
    }
}