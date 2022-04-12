using UnityEngine;
using System;
using UnityEngine.Events;

namespace DefaultNamespace.Manager
{
    public class LevelEvents : Singleton<LevelEvents>
    {
        // Unlike the Game Events this is a local version which is only used by the current level
        public UnityEvent<int> onTriggerEnter;
        public UnityEvent<int> onTriggerExit;
        public UnityEvent<int> onTriggerStay;
        
        public void TriggerEnter(int id)
        {
            if (onTriggerEnter != null)
            {
                onTriggerEnter.Invoke(id);
            }
        }
        
        
        public void TriggerStay(int id)
        {
            if (onTriggerStay != null)
            {
                onTriggerStay.Invoke(id);
            }
        }
        
        public void TriggerExit(int buttonId)
        {
            if (onTriggerExit != null)
            {
                onTriggerExit.Invoke(buttonId);
            }
        }
        
    }
}