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
            NextDestination = (Vector2)position + originalDirection * 0.5f;
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

            // check if it is possible to go in the direction of the last input
            if (_lastInput != Vector2.zero && !DetectWallBorderLastInput(_lastInput))
            {
                NextDestination = position + _lastInput / 2;
                animator.SetBool(IsMoving, true);
                _lastDirection = _lastInput;
                RotateRenderer();
            }
            // if there is no wall in the current direction then continue in the same direction
            else if (!DetectWallBorder(_lastDirection))
            {
                NextDestination = position + _lastDirection / 2;
                animator.SetBool(IsMoving, true);
            }
            // Stop pacman because there is a wall in the current direction
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
            // var linecast = Physics2D.Linecast(pos + dir, pos); // Detect a wall or border using linecast and tags
            return tilemap.HasTile(cellPosition);// || linecast.collider.CompareTag(tilemap.tag);
        }

        private bool DetectWallBorderLastInput(Vector2 dir)
        {
            var pos = (Vector2)transform.position;
            var posDir = pos + dir;

            if (dir.x != 0)
            {
                if (pos.y % 1 == 0)
                {
                    // if the player is centered in a tile check the tile in the direction of the input
                    return DetectWallBorder(dir);
                }
                else
                {
                    // if the player is not centered in a tile check two tiles in the direction of the input
                    var tile1 = tilemap.WorldToCell(posDir + Vector2.down / 2);
                    var tile2 = tilemap.WorldToCell(posDir + Vector2.up / 2);
                    return tilemap.HasTile(tile1) || tilemap.HasTile(tile2);
                }
            }
            else if (dir.y != 0)
            {
                if (pos.x % 1 == 0)
                {
                    // if the player is centered in a tile check the tile in the direction of the input
                    return DetectWallBorder(dir);
                }
                else
                {
                    // if the player is not centered in a tile check two tiles in the direction of the input
                    var tile1 = tilemap.WorldToCell(posDir + Vector2.left / 2);
                    var tile2 = tilemap.WorldToCell(posDir + Vector2.right / 2);
                    return tilemap.HasTile(tile1) || tilemap.HasTile(tile2);
                }
            }

            return false;
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
