using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
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

        private Vector2 _destination;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            var position = transform.position;
            _destination = (Vector2)position + originalDirection * initPositionOffset;
            _spawnPosition = position;
            _lastDirection = originalDirection;
            RotateRenderer();
        }

        private void FixedUpdate()
        {
            // Move the player
            var position = (Vector2)transform.position;
            var positionVector = Vector2.MoveTowards(position, _destination, speed * Time.deltaTime);
            _rigidbody2D.MovePosition(positionVector);

            // Check if the player is centered in the tile
            var isCentered = position == _destination;
            if (!isCentered) return;

            // if is at the middle of a tile, has input and there is no wall in the direction of the input
            if (_lastInput != Vector2.zero && !DetectWallBorder(_lastInput))
            {
                _destination = position + _lastInput;
                _lastDirection = _lastInput;
                animator.SetBool(IsMoving, true);
                RotateRenderer();
            }
            // if is at the middle of a tile and there is no wall in the current direction then continue in the same direction
            else if (!DetectWallBorder(_lastDirection))
            {
                _destination = position + _lastDirection;
                animator.SetBool(IsMoving, true);
            }
            else
            {
                animator.SetBool(IsMoving, false);
            }
        }

        private void RotateRenderer()
        {
            var angle = Vector2.SignedAngle(Vector2.right, _lastDirection);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private bool DetectWallBorder(Vector2 dir)
        {
            var pos = (Vector2)transform.position;
            var cellPosition = tilemap.WorldToCell(pos + dir); // Detect a wall or border with using grid's tiles
            var linecast = Physics2D.Linecast(pos + dir, pos); // Detect a wall or border using linecast and tags
            return tilemap.HasTile(cellPosition) || linecast.collider.CompareTag(tilemap.tag);
        }

        private void OnMove(InputValue value)
        {
            _inputDirection = value.Get<Vector2>();

            if (_inputDirection.x != 0) _inputDirection.y = 0; // Create a priority for x movement

            if (_inputDirection != Vector2.zero)
                _lastInput = _inputDirection.normalized; // Normalize the output to be 1 or -1 not floating values
        }

        public void SetDestination(Vector2 destination)
        {
            _destination = destination;
        }
    }
}
