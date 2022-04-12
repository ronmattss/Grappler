using Cinemachine;
using DefaultNamespace.Manager;
using UnityEngine;

namespace Environment
{
    public class CameraTransitionInteractable : Interactable
    {
        [SerializeField]private CinemachineVirtualCamera primaryCamera;
        
        [SerializeField]private CinemachineVirtualCamera secondaryCamera;
        
        public override void Interact()
        {
            CameraTransitionManager.Instance.currentVirtualCamera = secondaryCamera == primaryCamera ?  secondaryCamera : primaryCamera;
            CameraTransitionManager.Instance.currentVirtualCamera.Follow = GameManager.Instance.player.transform;
        }
    }
}