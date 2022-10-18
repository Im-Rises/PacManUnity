using UnityEngine;

namespace Door
{
    public class OpenCloseDoor : MonoBehaviour
    {
        private static readonly int IsOpen = Animator.StringToHash("isOpen");
        public Animator anim;
        public bool open;
        private Collider2D _col;

        private void Start()
        {
            anim.SetBool(IsOpen, open);
            _col = GetComponent<Collider2D>();
        }

        private void OpenDoor()
        {
            anim.SetBool(IsOpen, true);
            _col.enabled = false;
        }

        private void CloseDoor()
        {
            anim.SetBool(IsOpen, false);
            _col.enabled = true;
        }
    }
}
