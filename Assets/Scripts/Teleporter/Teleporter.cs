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

            if (other.gameObject.CompareTag(TagsConstants.PlayerTag))
                other.gameObject.GetComponent<PlayerController>().NextDestination = target.transform.position;
            else if (other.gameObject.CompareTag(TagsConstants.EnemyTag))
                other.gameObject.GetComponent<GhostAiMovement>()
                    .NextTileDestination = target.transform.position;
        }
    }
}
