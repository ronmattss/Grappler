using UnityEngine;

namespace Environment
{
    public class Interactable : MonoBehaviour, IOnPlayerInteract
    {
        
        public virtual void Interact()
        {
            Destroy(this.gameObject);
        }
        public void OnPlayerInteract()
        {
         Interact();   
        }
        
        
    }
}