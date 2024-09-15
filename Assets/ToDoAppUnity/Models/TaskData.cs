﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ToDoAppUnity.Tasks;

namespace ToDoAppUnity.Models
{
    /// <summary>
    /// Data model object representing a Task
    /// </summary>
    public class TaskData
    {
        public TaskData(string name, TaskDataMessage taskDataMessage, ICollection<TaskData> subTasks)
        {
            this.Name = name;
            this.TaskDataMessage = taskDataMessage;
            this.SubTasks = subTasks;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
        
        [JsonProperty("id")]
        public long Id { get; set; }
        
        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("taskItemMessage")]
        public TaskDataMessage? TaskDataMessage { get; set; }
        
        [JsonProperty("taskDataState")]
        public TaskDataState TaskDataState { get; set; } = TaskDataState.OPEN;
        
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
        
        [JsonProperty("completedAt")]
        public DateTime? CompletedAt { get; set; }
        
        [JsonProperty("deadlineAt")]
        public DateTime? DeadlineAt { get; set; }
        
        [JsonProperty("subTasks")]
        public ICollection<TaskData>? SubTasks { get; set; } = null;
        
        [JsonProperty("parentTaskId")]
        public long? ParentTaskId { get; set; } = null;

        public override bool Equals(object obj)
        {
            if (obj is not TaskData taskData)
            {
                return false;
            }
            return this.Id == taskData.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}