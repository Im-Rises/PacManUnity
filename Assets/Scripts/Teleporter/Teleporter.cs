using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Transform>().position = target.GetComponent<Transform>().position;
    }
}
