using System;
using TMPro;
using ToDoAppUnity.Models;
using UnityEngine;
using UnityEngine.UI;

namespace ToDoAppUnity.Tasks.UI
{
    /// <summary>
    /// UIRepresentation of TaskData
    /// </summary>
    public class TaskItem : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _taskNameInputField;
        [SerializeField] private Button _finishButton;
        [SerializeField] private Button _deleteButton;
        
        private TaskItemManager _parentTaskItemManager;
        private string _cachedValue;

        private TaskData TaskData { get; set; }

        private void OnEnable()
        {
            _taskNameInputField.onSelect.AddListener(CacheOldNameValue);
            _taskNameInputField.onSubmit.AddListener(UpdateTaskName);
            _deleteButton.onClick.AddListener(OnDeleteClicked);
        }

        private void OnDisable()
        {
            _taskNameInputField.onSelect.RemoveListener(CacheOldNameValue);
            _taskNameInputField.onSubmit.RemoveListener(UpdateTaskName);
            _deleteButton.onClick.RemoveListener(OnDeleteClicked);
        }

        public void Initialize(TaskData taskData, TaskItemManager taskItemManager)
        {
            TaskData = taskData;
            _parentTaskItemManager = taskItemManager;
            SetUIData(taskData);
        }

        private void SetUIData(TaskData taskData)
        {
            _taskNameInputField.text = taskData.Name;
        }

        private void CacheOldNameValue(string currentString)
        {
            _cachedValue = currentString;
        }
        
        private void UpdateTaskName(string newNameValue)
        {
            TaskData.Name = newNameValue;
            StartCoroutine(_parentTaskItemManager.ApiCaller.UpdateTaskItemAsync(TaskData.Id, TaskData, OnSuccess, OnFail));

            void OnSuccess(TaskData taskData)
            {
                TaskData = taskData;
                Debug.Log($"Success updating: {taskData.Name}");
            }

            void OnFail(Exception exception)
            {
                TaskData.Name = _cachedValue;
                _taskNameInputField.text = _cachedValue;
                Debug.LogError($"Couldn't update task item: {exception.Message}");
            }
        }
        
        private void OnDeleteClicked()
        {
            StartCoroutine(_parentTaskItemManager.ApiCaller.DeleteTaskItemAsync(TaskData.Id, OnSuccess, OnFail));
            
            void OnSuccess(TaskData taskData)
            {
                Debug.Log($"Success Deleting: {TaskData.Name}");
                _parentTaskItemManager.DeleteTaskItem(this);
            }

            void OnFail(Exception exception)
            {
                Debug.LogError($"Couldn't Delete item: {exception.Message}");
            }
        }
    }
}