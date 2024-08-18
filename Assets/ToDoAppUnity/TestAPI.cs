using System.Collections.Generic;
using JorisHoef.API.Services;
using ToDoAppUnity.Models;
using UnityEngine;

namespace ToDoAppUnity
{
    public class TestAPI : MonoBehaviour
    {
        private readonly ApiServices _apiServices = new ApiServices();
        
        private void OnEnable()
        {
            this.GetTaskItemAsync();
            this.GetAllTaskItemsAsync();
        }

        private void GetTaskItemAsync()
        {
            string endPoint = "https://localhost:5001/api/Taskitems/7";
            StartCoroutine(this._apiServices.GetAsync<TaskItem>(endPoint, false).AsIEnumerator());
        }

        private void GetAllTaskItemsAsync()
        {
            string endPoint = "https://localhost:5001/api/Taskitems";
            StartCoroutine(this._apiServices.GetAsync<List<TaskItem>>(endPoint, false).AsIEnumerator());
        }

        private void PostNewTaskItemAsync()
        {
            string endPoint = "https://localhost:5001/api/Taskitems";
            var taskItem = TaskItemManager.AddNewTaskItemMessage();
            StartCoroutine(this._apiServices.PostAsync<TaskItem>(endPoint, taskItem, false).AsIEnumerator());
        }

        private void UpdateTaskItemAsync()
        {
            string endPoint = "https://localhost:5001/api/Taskitems/7";
            var taskItem = TaskItemManager.AddNewTaskItemMessage(); //TODO: change to get an existing taskItem in our list
            StartCoroutine(this._apiServices.PutAsync<TaskItem>(endPoint, taskItem, false).AsIEnumerator());
        }

        private void DeleteTaskItemAsync()
        {
            string endPoint = "https://localhost:5001/api/Taskitems/7";
            StartCoroutine(this._apiServices.DeleteAsync<TaskItem>(endPoint, false).AsIEnumerator());
        }
    }
}