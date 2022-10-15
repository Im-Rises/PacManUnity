using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 10f;
        public string colliderTag = "WallsBorders";

        // public Vector2 step = new(1f, 1f);
        // public float initPositionOffset = 0.5f;
        private Vector2 _destination;

        private Vector2 _inputDirection;

        // private bool _isMoving;
        private Vector2 _lastInput;
        private Rigidbody2D _rigidbody2D;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _destination = transform.position;
        }

        private void FixedUpdate()
        {
            var positionVector = Vector2.MoveTowards(transform.position, _destination, speed * Time.deltaTime);
            _rigidbody2D.MovePosition(positionVector);

            var isCentered = (Vector2)transform.position == _destination;

            // if is at the middle of a tile, has input and there is no wall in the direction of the input
            if (isCentered && _inputDirection != Vector2.zero && !DetectWallBorder(_inputDirection))
            {
                _destination = (Vector2)transform.position + _inputDirection;
                _lastInput = _inputDirection;
            }
            // if is at the middle of a tile and there is no wall in the direction of the last input
            else if (isCentered && !DetectWallBorder(_lastInput))
            {
                _destination = (Vector2)transform.position + _lastInput;
            }
        }

        private bool DetectWallBorder(Vector2 dir)
        {
            Vector2 pos = transform.position;
            var hit = Physics2D.Linecast(pos + dir, pos);
            return hit.collider.CompareTag(colliderTag);
        }

        private void OnMove(InputValue value)
        {
            _inputDirection = value.Get<Vector2>();
        }
    }
}
