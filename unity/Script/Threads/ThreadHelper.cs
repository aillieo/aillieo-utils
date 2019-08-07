using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils
{
    public class ThreadHelper : SingletonMonoBehaviour<ThreadHelper>
    {
        readonly ConcurrentQueue<Action> actionQueue = new ConcurrentQueue<Action>();

        [RuntimeInitializeOnLoadMethod]
        private static void OnGameLoad()
        {
            CreateInstance();
        }

        public void QueueMainThread(Action action)
        {
            actionQueue.Enqueue(action); 
        }


        private void Update()
        {
            while (actionQueue.Count > 0)
            {
                Action action;

                if (actionQueue.TryDequeue(out action))
                {
                    action.Invoke();
                }
            }
        }
    }
}
