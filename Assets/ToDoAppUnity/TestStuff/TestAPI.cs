using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JorisHoef.API;
using JorisHoef.API.Services;
using ToDoAppUnity.Models;
using ToDoAppUnity.Tasks;
using UnityEngine;

namespace ToDoAppUnity.TestStuff
{
    public class TestAPI : MonoBehaviour
    {
        private readonly ApiServices _apiServices = new ApiServices();
        
        private string TestApiUrlSsl => TestAPIConstants.TEST_API_URL_SSL;
        private static string TestApiurl => TestAPIConstants.TEST_API_URL;
        private static string TestAPITaskitems => TestAPIConstants.TEST_API_TASKITEMS;
        
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
            string endPoint = $"{TestApiurl}/{TestAPITaskitems}/{idToGet}";
            yield return this._apiServices.GetAsync<TaskData>(endPoint, false).AsIEnumeratorWithCallback(OnCompleted);
            
            void OnCompleted(ApiCallResult<TaskData> apiCallResult)
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
            string endPoint = $"{TestApiurl}/{TestAPITaskitems}";
            yield return this._apiServices.GetAsync<List<TaskData>>(endPoint, false).AsIEnumeratorWithCallback(OnCompleted);

            void OnCompleted(ApiCallResult<List<TaskData>> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    Debug.Log($"API Call Completed: {apiCallResult.Data.Count}");
                    foreach (var taskItem in apiCallResult.Data)
                    {
                        Debug.Log($"ID: {taskItem.Id}, Name: {taskItem.Name}, Description: {taskItem.TaskDataMessage?.Message}");
                    }
                    TaskDataManager.AllTaskItems.Clear();
                    TaskDataManager.AllTaskItems.AddRange(apiCallResult.Data);
                }
                else
                {
                    
                }
            }
        }

        private IEnumerator PostNewTaskItemAsync()
        {
            string endPoint = $"{TestApiurl}/{TestAPITaskitems}";
            var taskItem = TaskDataManager.AddNewTaskItemMessage();
            yield return this._apiServices.PostAsync<TaskData>(endPoint, taskItem, false).AsIEnumeratorWithCallback(OnCompleted);
            
            void OnCompleted(ApiCallResult<TaskData> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    TaskDataManager.AllTaskItems.Add(apiCallResult.Data);
                    Debug.Log($"Successfully created task item: {apiCallResult.Data.Name}");
                }
                else
                {
                    
                }
            }
        }
        
        private IEnumerator UpdateTaskItemAsync(long idToUpdate)
        {
            string endPoint = $"{TestApiurl}/{TestAPITaskitems}/{idToUpdate}";
            TaskData taskItem = TaskDataManager.AllTaskItems.FirstOrDefault(x => x.Id == idToUpdate);
            taskItem.TaskDataMessage.Message = "updated Task Item Test";
            yield return this._apiServices.PutAsync<TaskData>(endPoint, taskItem, false).AsIEnumeratorWithCallback(OnCompleted);
            
            void OnCompleted(ApiCallResult<TaskData> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    taskItem.UpdatedAt = apiCallResult.Data.UpdatedAt;
                    Debug.Log($"Successfully updated task item {apiCallResult.Data.Name}");
                }
                else
                {
                    
                }
            }
        }
        
        private IEnumerator DeleteTaskItemAsync(long idToDelete)
        {
            string endPoint = $"{TestApiurl}/{TestAPITaskitems}/{idToDelete}";
            yield return this._apiServices.DeleteAsync<TaskData>(endPoint, false).AsIEnumeratorWithCallback(OnCompleted);
            
            void OnCompleted(ApiCallResult<TaskData> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    var deletedTaskItem = TaskDataManager.AllTaskItems.FirstOrDefault(x => x.Id == idToDelete);
                    TaskDataManager.AllTaskItems.Remove(deletedTaskItem);
                    Debug.Log($"Successfully Deleted task item with ID {idToDelete}");
                }
                else
                {
                    
                }
            }
        }
    }
}