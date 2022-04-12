using System;
using UnityEngine;

namespace DefaultNamespace.Collectibles
{
    public class Collectible : MonoBehaviour
    {
        // OnPickup will invoke when this collectible is collected
        public event Action<Collectible> OnPickup;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // some logic, probably an event handler
                OnPickup?.Invoke(this);
                this.gameObject.SetActive(false);
            }
        }
    }
}