namespace Environment
{
    using UnityEngine;
    public interface IOnPlayerInteract 
    {
        /// <summary> any interaction with the player Invokes OnPlayerInteract, spikes, collectible, etc</summary>
        void OnPlayerInteract();
    }
}