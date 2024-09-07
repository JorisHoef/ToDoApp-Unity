using System;
using JorisHoef.UI;
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
        [SerializeField] private Button _taskStateButton;
        [SerializeField] private Button _dropDownButton;
        [SerializeField] private Button _deleteButton;
        [SerializeField] private VisibilityController _taskDetailContainer;
        [SerializeField] private TaskItemDetails _taskDetailsContainer;
        
        private TaskItemManager _parentTaskItemManager;
        private string _cachedNameValue;

        private TaskData TaskData { get; set; }

        private void OnEnable()
        {
            _taskNameInputField.onSelect.AddListener(CacheOldNameValue);
            _taskNameInputField.onSubmit.AddListener(UpdateTaskName);
            _deleteButton.onClick.AddListener(OnDeleteClicked);
            _taskStateButton.onClick.AddListener(OnTaskStateClicked);
            _dropDownButton.onClick.AddListener(OnDropdownClicked);
        }

        private void OnDisable()
        {
            _taskNameInputField.onSelect.RemoveListener(CacheOldNameValue);
            _taskNameInputField.onSubmit.RemoveListener(UpdateTaskName);
            _deleteButton.onClick.RemoveListener(OnDeleteClicked);
            _taskStateButton.onClick.RemoveListener(OnTaskStateClicked);
            _dropDownButton.onClick.RemoveListener(OnDropdownClicked);
        }

        public void Initialize(TaskData taskData, TaskItemManager taskItemManager)
        {
            TaskData = taskData;
            _parentTaskItemManager = taskItemManager; 
            _taskDetailsContainer.Initialize(this, taskData, _parentTaskItemManager);
            SetUIData(taskData);
        }
        
        private void SetUIData(TaskData taskData)
        {
            _taskNameInputField.text = taskData.Name;
        }

        private void CacheOldNameValue(string currentString)
        {
            _cachedNameValue = currentString;
        }
        
        private void UpdateTaskName(string newNameValue)
        {
            TaskData.Name = newNameValue;
            UpdateTaskItem(TaskData);
        }
        
        private void OnDropdownClicked()
        {
            _taskDetailContainer.SetState();
        }
        
        private void OnTaskStateClicked()
        {
            TaskData.TaskDataState = TaskData.TaskDataState switch
            {
                    TaskDataState.OPEN   => TaskDataState.CLOSED,
                    TaskDataState.CLOSED => TaskDataState.OPEN,
                    TaskDataState.STALE => TaskDataState.CLOSED,
                    _                    => TaskData.TaskDataState
            };
            
            UpdateTaskItem(TaskData);
        }

        public void UpdateTaskItem(TaskData taskData)
        {
            StartCoroutine(_parentTaskItemManager.ApiCaller.UpdateTaskItemAsync(taskData.Id, taskData, OnSuccess, OnFail));
            
            void OnSuccess(TaskData returnedTaskData)
            {
                TaskData = returnedTaskData;
                _taskDetailsContainer.Initialize(this, TaskData, _parentTaskItemManager);
                Debug.Log($"Success updating: {returnedTaskData.Name} with state {returnedTaskData.TaskDataState}");
            }

            void OnFail(Exception exception)
            {
                TaskData.Name = _cachedNameValue;
                _taskNameInputField.text = _cachedNameValue;
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