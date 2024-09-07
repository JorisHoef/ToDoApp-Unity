using System;
using System.Collections.Generic;
using ToDoAppUnity.Models;
using UnityEngine;
using UnityEngine.UI;

namespace ToDoAppUnity.Tasks.UI
{
    /// <summary>
    /// Manages all UI representations of TaskItems
    /// </summary>
    public class TaskItemManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _taskItemContainer;
        [SerializeField] private TaskItem _taskItemPrefab;
        [SerializeField] private Button _createTaskButton;
        
        private readonly TaskApiCaller _taskApiCaller = new TaskApiCaller();
        private readonly TaskCreator _taskCreator = new TaskCreator();
        
        public TaskApiCaller ApiCaller => _taskApiCaller;
        
        private void Awake()
        {
            SetupAllTaskDatas();
        }

        private void OnEnable()
        {
            _createTaskButton.onClick.AddListener(OnCreateTaskClicked);
        }

        private void OnDisable()
        {
            _createTaskButton.onClick.RemoveListener(OnCreateTaskClicked);
        }

        public void DeleteTaskItem(TaskItem taskItem)
        {
            GameObject.Destroy(taskItem.gameObject);
        }
        
        private void SetupAllTaskDatas()
        {
            StartCoroutine(_taskApiCaller.GetAllTaskItemsAsync(OnSuccess, OnFail));
            
            void OnSuccess(List<TaskData> taskDatas)
            { 
                PopulateTasksList(taskDatas);
            }

            void OnFail(Exception exception)
            {
                Debug.LogError($"Failed to create task list: {exception}");
            }
        }
        
        private void PopulateTasksList(List<TaskData> taskDatas)
        {
            foreach (var taskData in taskDatas)
            {
                SetupTaskItem(taskData, _taskItemContainer);
            }
        }
        
        private void OnCreateTaskClicked()
        {
            var taskData = _taskCreator.CreateTask();
            AddTaskItem(taskData, _taskItemContainer);
        }
        
        public void AddTaskItem(TaskData taskData, RectTransform taskContainer)
        {
            StartCoroutine(_taskApiCaller.PostNewTaskItemAsync(taskData, OnSuccess, OnFail));
            
            void OnSuccess(TaskData dataResponse)
            {
                Debug.Log($"Successfully created task: {dataResponse.Name} {dataResponse.TaskDataState}");
                SetupTaskItem(dataResponse, taskContainer);
            }

            void OnFail(Exception exception)
            {
                Debug.LogError($"Failed to create task item {exception}");
            }
        }

        private void SetupTaskItem(TaskData taskData, RectTransform taskContainer)
        {
            TaskItem newTaskItem = GameObject.Instantiate(_taskItemPrefab, taskContainer);
            newTaskItem.Initialize(taskData, this);
        }
    }
}