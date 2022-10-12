using UnityEngine;

public class OpenCloseDoor : MonoBehaviour
{
    public Animator anim;
    public bool open;
    private Collider2D col;

    private void Start()
    {
        anim.SetBool("isOpen", open);
        col = GetComponent<Collider2D>();
    }

    private void OpenDoor()
    {
        anim.SetBool("isOpen", true);
        col.enabled = false;
    }

    private void CloseDoor()
    {
        anim.SetBool("isOpen", false);
        col.enabled = true;
    }
}
