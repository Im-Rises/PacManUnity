using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash(AnimationsConstants.PlayerIsMoving);
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

        public Vector2 NextDestination { get; set; }

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            var position = transform.position;
            NextDestination = (Vector2)position + originalDirection * initPositionOffset;
            _spawnPosition = position;
            _lastDirection = originalDirection;
            RotateRenderer();
        }

        private void FixedUpdate()
        {
            // Move the player
            var position = (Vector2)transform.position;

            var positionVector = Vector2.MoveTowards(position, NextDestination, speed * Time.deltaTime);
            _rigidbody2D.MovePosition(positionVector);

            // Check if the player is centered in the tile
            var isAtDestination = position == NextDestination;
            if (!isAtDestination) return;

            // if is at the middle of a tile, has input and there is no wall in the direction of the input
            if (_lastInput != Vector2.zero && !DetectWallBorder(_lastInput))
            {
                NextDestination = position + _lastInput;
                _lastDirection = _lastInput;
                animator.SetBool(IsMoving, true);
                RotateRenderer();
            }
            // if is at the middle of a tile and there is no wall in the current direction then continue in the same direction
            else if (!DetectWallBorder(_lastDirection))
            {
                NextDestination = position + _lastDirection;
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
            var posDir = pos + dir;

            // If at the center of a tile check if there is a wall in the direction of the input
            var cellPosition = tilemap.WorldToCell(posDir); // Detect a wall or border with using grid's tiles
            var linecast = Physics2D.Linecast(posDir, pos); // Detect a wall or border using linecast and tags
            return tilemap.HasTile(cellPosition) || linecast.collider.CompareTag(tilemap.tag);


            // var pos = (Vector2)transform.position;
            // var posDir = pos + dir;
            //
            // if (dir.x != 0)
            // {
            //     if (pos.x % 1 == 0 && pos.y % 1 == 0)
            //     {
            //         // If at the center of a tile check if there is a wall in the direction of the input
            //         return hasTile(posDir);
            //     }
            //
            //     // If is between two tiles check two tiles in the direction of the input
            //     Vector2 posDir2 = posDir + dir;
            //     return hasTile(posDir) || hasTile(posDir2);
            // }
            //
            // if (dir.y != 0)
            // {
            //     if (pos.x % 1 == 0 && pos.y % 1 == 0)
            //     {
            //         return hasTile(posDir);
            //     }
            //
            //     // If is between two tiles check two tiles in the direction of the input
            //     Vector2 posDir2 = posDir + dir;
            //     return hasTile(posDir) || hasTile(posDir2);
            // }
            //
            // return true;
        }

        // private bool hasTile(Vector2 pos)
        // {
        //     var cellPosition = tilemap.WorldToCell(pos);
        //     return tilemap.HasTile(cellPosition);
        // }

        private void OnMove(InputValue value)
        {
            _inputDirection = value.Get<Vector2>();

            if (_inputDirection.x != 0) _inputDirection.y = 0; // Create a priority for x movement

            if (_inputDirection != Vector2.zero)
                _lastInput = _inputDirection.normalized; // Normalize the output to be 1 or -1 not floating values
        }
    }
}
