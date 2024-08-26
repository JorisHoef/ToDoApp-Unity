using System;
using System.Collections.Generic;
using ToDoAppUnity.Models;
using UnityEngine;

namespace ToDoAppUnity.Tasks.UI
{
    public class TaskItemManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _taskItemContainer;
        [SerializeField] private TaskItem _taskItemPrefab;

        private readonly TaskApiCaller _taskApiCaller = new TaskApiCaller();
        
        public TaskApiCaller ApiCaller => _taskApiCaller;
        
        private void Awake()
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
                TaskItem newTaskItem = GameObject.Instantiate(_taskItemPrefab, _taskItemContainer);
                newTaskItem.Initialize(taskData, this);
            }
        }

        public void DeleteTaskItem(TaskItem taskitem)
        {
            GameObject.Destroy(taskitem.gameObject);
        }
    }
}