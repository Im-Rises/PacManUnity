using UnityEngine;

namespace Door
{
    public class DoorHandler : MonoBehaviour
    {
        public Animator anim;

        // public SpriteRenderer spriteRenderer;

        // public Sprite doorOpen;
        // public Sprite doorClosed;

        // public bool isOpen = true;

        private void Start()
        {
            Invoke(nameof(OpenDoor), 4f);
        }

        public void OpenDoor()
        {
            anim.SetBool(AnimationsConstants.DoorIsOpen, true);
        }

        public void CloseDoor()
        {
            Invoke(nameof(CloseDoorInvoke), 1.0f);
        }

        private void CloseDoorInvoke()
        {
            anim.SetBool(AnimationsConstants.DoorIsOpen, false);
        }
    }
}
