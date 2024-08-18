using System.Collections.Generic;
using JorisHoef.API;
using JorisHoef.API.Services;
using ToDoAppUnity.Models;
using UnityEngine;

namespace ToDoAppUnity
{
    public class TestAPI : MonoBehaviour
    {
        private readonly ApiServices _apiServices = new ApiServices();
        private const string TEST_API_URL = "https://localhost:5001/api";
        private const string TEST_API_TASKITEMS = "taskitems";
        private const int TEST_ID = 1;
        
        private void OnEnable()
        {
            this.GetAllTaskItemsAsync();
        }

        private void GetTaskItemAsync()
        {
            string endPoint = $"{TEST_API_URL}/{TEST_API_TASKITEMS}/{TEST_ID}";
            StartCoroutine(this._apiServices.GetAsync<TaskItem>(endPoint, false).AsIEnumeratorWithCallback(OnCompleted));
            
            void OnCompleted(ApiCallResult<TaskItem> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    
                }
                else
                {
                    
                }
            }
        }

        private void GetAllTaskItemsAsync()
        {
            string endPoint = $"{TEST_API_URL}/{TEST_API_TASKITEMS}";
            StartCoroutine(this._apiServices.GetAsync<List<TaskItem>>(endPoint, false).AsIEnumeratorWithCallback(OnCompleted));

            void OnCompleted(ApiCallResult<List<TaskItem>> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    
                }
                else
                {
                    
                }
            }
        }

        private void PostNewTaskItemAsync()
        {
            string endPoint = $"{TEST_API_URL}/{TEST_API_TASKITEMS}";
            var taskItem = TaskItemManager.AddNewTaskItemMessage();
            StartCoroutine(this._apiServices.PostAsync<TaskItem>(endPoint, taskItem, false).AsIEnumeratorWithCallback(OnCompleted));
            
            void OnCompleted(ApiCallResult<TaskItem> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    
                }
                else
                {
                    
                }
            }
        }
        
        private void UpdateTaskItemAsync()
        {
            string endPoint = $"{TEST_API_URL}/{TEST_API_TASKITEMS}/{TEST_ID}";
            var taskItem = TaskItemManager.AddNewTaskItemMessage(); //TODO: change to get an existing taskItem in our list
            StartCoroutine(this._apiServices.PutAsync<TaskItem>(endPoint, taskItem, false).AsIEnumeratorWithCallback(OnCompleted));
            
            void OnCompleted(ApiCallResult<TaskItem> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    
                }
                else
                {
                    
                }
            }
        }
        
        private void DeleteTaskItemAsync()
        {
            string endPoint = $"{TEST_API_URL}/{TEST_API_TASKITEMS}/{TEST_ID}";
            StartCoroutine(this._apiServices.DeleteAsync<TaskItem>(endPoint, false).AsIEnumeratorWithCallback(OnCompleted));
            
            void OnCompleted(ApiCallResult<TaskItem> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    
                }
                else
                {
                    
                }
            }
        }
    }
}