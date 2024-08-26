using System;
using System.Collections;
using System.Collections.Generic;
using JorisHoef.API;
using JorisHoef.API.Services;
using ToDoAppUnity.Models;
using ToDoAppUnity.TestStuff;

namespace ToDoAppUnity.Tasks
{
    public class TaskApiCaller
    {
        private readonly ApiServices _apiServices = new ApiServices();
        
        private string TestApiUrlSsl => TestAPIConstants.TEST_API_URL_SSL;
        private static string TestApiUrl => TestAPIConstants.TEST_API_URL;
        private static string TestAPITaskitems => TestAPIConstants.TEST_API_TASKITEMS;
        
        public IEnumerator GetTaskItemAsync(long idToGet, Action<TaskData> onSuccess, Action<Exception> onFail)
        {
            string endPoint = $"{TestApiUrl}/{TestAPITaskitems}/{idToGet}";
            yield return this._apiServices.GetAsync<TaskData>(endPoint, false).AsIEnumeratorWithCallback(OnCompleted);
            
            void OnCompleted(ApiCallResult<TaskData> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    onSuccess?.Invoke(apiCallResult.Data);
                }
                else
                {
                    onFail?.Invoke(apiCallResult.Exception);
                }
            }
        }
        
        public IEnumerator GetAllTaskItemsAsync(Action<List<TaskData>> onSuccess, Action<Exception> onFail)
        {
            string endPoint = $"{TestApiUrl}/{TestAPITaskitems}";
            yield return this._apiServices.GetAsync<List<TaskData>>(endPoint, false).AsIEnumeratorWithCallback(OnCompleted);
            
            void OnCompleted(ApiCallResult<List<TaskData>> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    onSuccess?.Invoke(apiCallResult.Data);
                }
                else
                {
                    onFail?.Invoke(apiCallResult.Exception);
                }
            }
        }
        
        public IEnumerator PostNewTaskItemAsync(TaskData taskData, Action<TaskData> onSuccess, Action<Exception> onFail)
        {
            string endPoint = $"{TestApiUrl}/{TestAPITaskitems}";
            yield return this._apiServices.PostAsync<TaskData>(endPoint, taskData, false).AsIEnumeratorWithCallback(OnCompleted);
            
            void OnCompleted(ApiCallResult<TaskData> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    onSuccess?.Invoke(apiCallResult.Data);
                }
                else
                {
                    onFail?.Invoke(apiCallResult.Exception);
                }
            }
        }
        
        public IEnumerator UpdateTaskItemAsync(long idToUpdate, TaskData taskData, Action<TaskData> onSuccess, Action<Exception> onFail)
        {
            string endPoint = $"{TestApiUrl}/{TestAPITaskitems}/{idToUpdate}";
            yield return this._apiServices.PutAsync<TaskData>(endPoint, taskData, false).AsIEnumeratorWithCallback(OnCompleted);
            
            void OnCompleted(ApiCallResult<TaskData> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                {
                    onSuccess?.Invoke(apiCallResult.Data);
                }
                else
                {
                    onFail?.Invoke(apiCallResult.Exception);
                }
            }
        }
        
        public IEnumerator DeleteTaskItemAsync(long idToDelete, Action<TaskData> onSuccess, Action<Exception> onFail)
        {
            string endPoint = $"{TestApiUrl}/{TestAPITaskitems}/{idToDelete}";
            yield return this._apiServices.DeleteAsync<TaskData>(endPoint, false).AsIEnumeratorWithCallback(OnCompleted);
            
            void OnCompleted(ApiCallResult<TaskData> apiCallResult)
            {
                if (apiCallResult.IsSuccess)
                { 
                    onSuccess?.Invoke(apiCallResult.Data);
                }
                else
                { 
                    onFail?.Invoke(apiCallResult.Exception);
                }
            }
        }
    }
}