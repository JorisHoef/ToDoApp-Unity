﻿using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace JorisHoef.API.Services
{
    public static class TaskExtensions
    {
        public static IEnumerator AsIEnumerator<T>(this Task<T> task, System.Action<T> onCompleted = null)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsCompletedSuccessfully)
            {
                onCompleted?.Invoke(task.Result);
            }
            else if (task.IsFaulted)
            {
                Debug.LogError($"Task failed with exception: {task.Exception}");
            }
        }
    }
}