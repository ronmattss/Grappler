using System;
using UnityEngine;

namespace DefaultNamespace.Manager
{
    public class EventTrigger : MonoBehaviour
    {
        // invokes the event

        public int eventID;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Event Triggered");
            LevelEvents.Instance.TriggerEnter(eventID);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Event Triggered");
            LevelEvents.Instance.TriggerExit(eventID);
        }
    }
}