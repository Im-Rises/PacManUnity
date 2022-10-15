using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 10f;
        public string colliderTag = "WallsBorders";
        private CircleCollider2D _circleCollider;
        private Vector2 _inputDirection;
        private Vector2 _lastDirection;
        private Vector2 _lastInput;
        private Rigidbody2D _rigidbody2D;

        // public Vector2 step = new(1f, 1f);
        // public float initPositionOffset = 0.5f;
        private Vector2 _destination { get; set; }

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
            if (isCentered && _lastInput != Vector2.zero && !DetectWallBorder(_lastInput))
            {
                _destination = (Vector2)transform.position + _lastInput;
                _lastDirection = _lastInput;
            }
            // if is at the middle of a tile and there is no wall in the current direction
            else if (isCentered && !DetectWallBorder(_lastDirection))
            {
                _destination = (Vector2)transform.position + _lastDirection;
            }
        }

        private bool DetectWallBorder(Vector2 dir)
        {
            Vector2 pos = transform.position;
            var centerHit = Physics2D.Linecast(pos + dir, pos);
            // var temp = new Vector2(dir.y, dir.x);
            var rearHit = Physics2D.Linecast(pos + dir + dir * _circleCollider.bounds.extents, pos);
            return centerHit.collider.CompareTag(colliderTag) || rearHit.collider.CompareTag(colliderTag);
        }

        private void OnMove(InputValue value)
        {
            _inputDirection = value.Get<Vector2>();
            if (_inputDirection != Vector2.zero) _lastInput = _inputDirection;
        }
    }
}
