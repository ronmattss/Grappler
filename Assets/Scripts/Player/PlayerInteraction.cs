using System;
using Environment;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerInteraction : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.name);
            if (other.gameObject.CompareTag("Interactable"))
            {
                other.GetComponent<IOnPlayerInteract>().OnPlayerInteract();

            }
        }
    }
}