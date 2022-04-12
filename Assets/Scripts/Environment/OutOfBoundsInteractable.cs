using DefaultNamespace.Manager;

namespace Environment
{
    public class OutOfBoundsInteractable : Interactable
    {
        public override void Interact()
        {
           GameManager.Instance.SetPlayerPosition();
        }
    }
}