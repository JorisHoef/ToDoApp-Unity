using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JorisHoef.API;
using JorisHoef.API.Services;
using ToDoAppUnity.Models;
using ToDoAppUnity.Tasks;
using UnityEngine;

namespace ToDoAppUnity
{
    public class TestAPI : MonoBehaviour
    {
        private readonly ApiServices _apiServices = new ApiServices();
        
        private const string TEST_API_URL_SSL = "https://localhost:5001/api";
        private const string TEST_API_URL = "http://localhost:5000/api";
        private const string TEST_API_TASKITEMS = "taskitems";
        private const long TEST_ID = 1;
        
        private void OnEnable()
        {
            StartCoroutine(TestAPISequence());
        }

        private IEnumerator TestAPISequence()
        {
            yield return PostNewTaskItemAsync();
            yield return PostNewTaskItemAsync();
            yield return GetAllTaskItemsAsync();
            yield return UpdateTaskItemAsync(2);
            yield return GetTaskItemAsync(2);
            yield return PostNewTaskItemAsync();
            yield return DeleteTaskItemAsync(3);
        }

        private IEnumerator GetTaskItemAsync(long idToGet)
        {
            string endPoint = $"{TEST_API_URL}/{TEST_API_TASKITEMS}/{idToGet}";
            yield return this._apiServices.GetAsync<TaskItem>(endPoint, false).AsIEnumeratorWithCallback(OnCompleted);
            
            void OnCompleted(ApiCallResult<TaskItem> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    Debug.Log($"Successfully retrieved task item {apiCallResult.Data.Name}");
                }
                else
                {
                    
                }
            }
        }

        private IEnumerator GetAllTaskItemsAsync()
        {
            string endPoint = $"{TEST_API_URL}/{TEST_API_TASKITEMS}";
            yield return this._apiServices.GetAsync<List<TaskItem>>(endPoint, false).AsIEnumeratorWithCallback(OnCompleted);

            void OnCompleted(ApiCallResult<List<TaskItem>> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    Debug.Log($"API Call Completed: {apiCallResult.Data.Count}");
                    foreach (var taskItem in apiCallResult.Data)
                    {
                        Debug.Log($"ID: {taskItem.Id}, Name: {taskItem.Name}, Description: {taskItem.TaskItemMessage?.Message}");
                    }
                    TaskItemManager.AllTaskItems.Clear();
                    TaskItemManager.AllTaskItems.AddRange(apiCallResult.Data);
                }
                else
                {
                    
                }
            }
        }

        private IEnumerator PostNewTaskItemAsync()
        {
            string endPoint = $"{TEST_API_URL}/{TEST_API_TASKITEMS}";
            var taskItem = TaskItemManager.AddNewTaskItemMessage();
            yield return this._apiServices.PostAsync<TaskItem>(endPoint, taskItem, false).AsIEnumeratorWithCallback(OnCompleted);
            
            void OnCompleted(ApiCallResult<TaskItem> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    TaskItemManager.AllTaskItems.Add(apiCallResult.Data);
                    Debug.Log($"Successfully created task item: {apiCallResult.Data.Name}");
                }
                else
                {
                    
                }
            }
        }
        
        private IEnumerator UpdateTaskItemAsync(long idToUpdate)
        {
            string endPoint = $"{TEST_API_URL}/{TEST_API_TASKITEMS}/{idToUpdate}";
            TaskItem taskItem = TaskItemManager.AllTaskItems.FirstOrDefault(x => x.Id == idToUpdate);
            taskItem.TaskItemMessage.Message = "updated Task Item Test";
            yield return this._apiServices.PutAsync<TaskItem>(endPoint, taskItem, false).AsIEnumeratorWithCallback(OnCompleted);
            
            void OnCompleted(ApiCallResult<TaskItem> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    Debug.Log($"Successfully updated task item {apiCallResult.Data.Name}");
                }
                else
                {
                    
                }
            }
        }
        
        private IEnumerator DeleteTaskItemAsync(long idToDelete)
        {
            string endPoint = $"{TEST_API_URL}/{TEST_API_TASKITEMS}/{idToDelete}";
            yield return this._apiServices.DeleteAsync<TaskItem>(endPoint, false).AsIEnumeratorWithCallback(OnCompleted);
            
            void OnCompleted(ApiCallResult<TaskItem> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    var deletedTaskItem = TaskItemManager.AllTaskItems.FirstOrDefault(x => x.Id == idToDelete);
                    TaskItemManager.AllTaskItems.Remove(deletedTaskItem);
                    Debug.Log($"Successfully Deleted task item with ID {idToDelete}");
                }
                else
                {
                    
                }
            }
        }
    }
}