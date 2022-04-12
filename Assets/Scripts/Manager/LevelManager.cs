using System;
using System.Collections.Generic;
using DefaultNamespace.Collectibles;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.Manager
{
    public class LevelManager : MonoBehaviour
    {
        //Manages collectibles and other entities in the level

        public List<Collectible> collectibles;
        [SerializeField] private GameObject collectibleObject;
        // Sample Event what happens when all collectibles is collected
         public UnityEvent OnCollectAll;
         public int collectiblesCollected = 0;
        public void Awake()
        {
            var availableCollectibles = collectibleObject.GetComponentsInChildren<Collectible>();
            collectibles = new List<Collectible>(availableCollectibles);
            
            foreach (var collectible in collectibles)
            {
                collectible.OnPickup += EventTest; // this is an action event from (C#) // use this if you want specific scripts to be notified when a collectible is picked up
            }
        }

        void EventTest(Collectible collectible)
        {
            Debug.Log($"Removing collectible: {collectible.name}");
            collectibles.Remove(collectible);
            collectiblesCollected++;
            UIManager.Instance.ChangeScoreText(collectiblesCollected);

            if (collectibles.Count == 0)
            {
                OnCollectAll?.Invoke(); // Invoke all subscribed functions to this event
                UIManager.Instance.ChangeScoreText("All Collectibles Collected");

            }
        }
        
        
    }
}