using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using ToDoAppUnity.Models;
using UnityEngine;
using UnityEngine.Networking;

namespace ToDoAppUnity
{
    public class TestAPI : MonoBehaviour
    {
        [ContextMenu("Test GetTaskItem")]
        private void GetTaskItem()
        {
            var lol = new ContextMenu("lol");
            string endPoint = "https://localhost:5001/api/Taskitems/7";
            UnityWebRequest webRequest = new UnityWebRequest(endPoint, "GET");
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            StartCoroutine(SendGetSingleRequest(webRequest));
        }
        
        [ContextMenu("Test GetAllTaskItems")]
        private void GetAllTaskItems()
        {
            UnityWebRequest webRequest = new UnityWebRequest("https://localhost:5001/api/Taskitems", "GET");
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            StartCoroutine(SendGetRequest(webRequest));
        }

        [ContextMenu("Test SendTaskItem")]
        private void SendNewTaskItem()
        {
            var taskItem = TaskItem.AddNewTaskItemMessage();
            
            string jsonPayload = JsonConvert.SerializeObject(taskItem);
            
            UnityWebRequest webRequest = new UnityWebRequest("https://localhost:5001/api/Taskitems", "POST");
            
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            
            StartCoroutine(SendRequest(webRequest));
        }

        private IEnumerator SendGetRequest(UnityWebRequest request)
        {
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                Debug.Log("Response: " + request.downloadHandler.text);
                List<TaskItem> taskItems = JsonConvert.DeserializeObject<List<TaskItem>>(request.downloadHandler.text);
                TaskItemManager.AllTaskItems = taskItems;
                
                foreach (var task in taskItems)
                {
                    Debug.Log("Task: " + task.Name);
                }
            }
            
            Honkie();
            Plankie();
        }

        private static void Honkie()
        {
            
        }

        private void Plankie()
        {
            
        }

        private IEnumerator SendGetSingleRequest(UnityWebRequest request)
        {
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                Debug.Log("Response: " + request.downloadHandler.text);
                TaskItem taskItem = JsonConvert.DeserializeObject<TaskItem>(request.downloadHandler.text);

                if (taskItem != null)
                {
                    Debug.Log("Task ID: " + taskItem.Id);
                    Debug.Log("Task Name: " + taskItem.Name);
                    Debug.Log("Task State: " + taskItem.TaskItemState);
                    // You can add more debug logs here as needed
                }
                else
                {
                    Debug.Log("TaskItem is null or deserialization failed.");
                }
            }
        }

        private IEnumerator SendRequest(UnityWebRequest request)
        {
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                Debug.Log("Response: " + request.downloadHandler.text);
            }
        }
    }
}