using Player;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject target;
    public string playerTag = "Player";
    public string enemyTag = "Enemy";

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Transform>().position = target.GetComponent<Transform>().position;

        if (other.gameObject.CompareTag(playerTag))
            other.gameObject.GetComponent<PlayerController>().Destination =
                target.GetComponent<Transform>().position;
        // else if (other.gameObject.CompareTag(enemyTag))
        //     other.gameObject.GetComponent<EnemyController>().Destination =
        //         target.GetComponent<Transform>().position;
    }
}
