using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 5f;
        private Vector3 _direction;

        private void Start()
        {
        }

        private void FixedUpdate()
        {
            transform.Translate(_direction * (speed * Time.deltaTime));
        }

        private void OnMove(InputValue value)
        {
            _direction = value.Get<Vector2>();
        }
    }
}
