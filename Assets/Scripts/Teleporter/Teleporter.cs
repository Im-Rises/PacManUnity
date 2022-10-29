using Ghosts;
using Player;
using UnityEngine;

namespace Teleporter
{
    public class Teleporter : MonoBehaviour
    {
        public GameObject target;

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<Transform>().position = target.GetComponent<Transform>().position;

            if (other.gameObject.CompareTag("Player"))
                other.gameObject.GetComponent<PlayerController>().SetDestination(target.transform.position);
            else if (other.gameObject.CompareTag("Ghost"))
                other.gameObject.GetComponent<GhostAiMovement>()
                    .SetNextTileDestination(target.transform.position);
        }
    }
}
