using UnityEngine;

namespace CameraMovement
{
    public class CameraMovemenThirdPerson : MonoBehaviour
    {
        public Transform player;
        public Vector3 posOffset;
        public float timeOffset;

        private Vector3 velocity;

        private void Update()
        {
            transform.position =
                Vector3.SmoothDamp(transform.position, player.position + posOffset, ref velocity, timeOffset);
        }
    }
}
