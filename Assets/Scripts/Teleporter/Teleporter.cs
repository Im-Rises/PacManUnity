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

            if (other.gameObject.CompareTag(TagsConstants.PLAYER_TAG))
                other.gameObject.GetComponent<PlayerController>().NextDestination = target.transform.position;
            else if (other.gameObject.CompareTag(TagsConstants.ENEMY_TAG))
                other.gameObject.GetComponent<GhostAiMovement>()
                    .NextTileDestination = target.transform.position;
        }
    }
}
