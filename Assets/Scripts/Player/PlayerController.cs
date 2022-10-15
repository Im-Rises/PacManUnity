using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 10f;

        public Tilemap tilemap;
        public Vector2 originalDirection = new(-1, 0);
        public float initPositionOffset = 0.5f;

        public Animator animator;
        private Vector2 _inputDirection;
        private Vector2 _lastDirection;
        private Vector2 _lastInput;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _spawnPosition;

        public Vector2 Destination { get; set; }

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            var position = transform.position;
            Destination = (Vector2)position + originalDirection * initPositionOffset;
            _spawnPosition = position;
            _lastDirection = originalDirection;
            RotateRenderer();
        }

        private void FixedUpdate()
        {
            var position = transform.position;
            var positionVector = Vector2.MoveTowards(position, Destination, speed * Time.deltaTime);
            _rigidbody2D.MovePosition(positionVector);

            var isCentered = (Vector2)position == Destination;

            if (!isCentered) return;

            // if is at the middle of a tile, has input and there is no wall in the direction of the input
            if (_lastInput != Vector2.zero && !DetectWallBorder(_lastInput))
            {
                Destination = (Vector2)transform.position + _lastInput;
                _lastDirection = _lastInput;
                animator.SetBool("isMoving", true);
                RotateRenderer();
            }
            // if is at the middle of a tile and there is no wall in the current direction then continue in the same direction
            else if (!DetectWallBorder(_lastDirection))
            {
                Destination = (Vector2)transform.position + _lastDirection;
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }

        private void RotateRenderer()
        {
            var angle = Vector2.SignedAngle(Vector2.right, _lastDirection);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private bool DetectWallBorder(Vector3 dir)
        {
            var pos = transform.position;
            var cellPosition = tilemap.WorldToCell(pos + dir);
            var linecast = Physics2D.Linecast(pos + dir, pos);
            return tilemap.HasTile(cellPosition) || linecast.collider.CompareTag(tilemap.tag);
        }

        private void OnMove(InputValue value)
        {
            _inputDirection = value.Get<Vector2>();

            if (_inputDirection.x != 0) _inputDirection.y = 0; // Create a priority for x movement

            if (_inputDirection != Vector2.zero)
                _lastInput = _inputDirection.normalized; // Normalize the output to be 1 or -1 not floating values
        }
    }
}
