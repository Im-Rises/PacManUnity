using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        // Player speed
        public float speed = 10f;

        // Animator
        public Animator animator;
        private static readonly int IsMoving = Animator.StringToHash(AnimationsConstants.PlayerIsMoving);

        // Rigidbody
        private Rigidbody2D _rigidbody2D;

        // Tilemap
        public Tilemap tilemap;

        // Init spawn position and direction
        // public Vector2 originalDirection = new(-1, 0);
        public Vector2 initDirection = Vector2.left / 2;
        private Vector2 _spawnPosition;

        // Player direction
        private Vector2 _inputDirection;
        private Vector2 _lastInputDirection;
        private Vector2 _lastDirection;


        public Vector2 NextDestination { get; set; }

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            var position = transform.position;
            _spawnPosition = position;
            _lastDirection = initDirection.normalized;

            // if (transform.position.x % 1 != 0 || transform.position.y % 1 != 0)
            //     NextDestination = (Vector2)position + originalDirection * 0.5f;
            // else
            //     NextDestination = (Vector2)position + originalDirection;

            RotateRenderer();
        }

        private void FixedUpdate()
        {
            // Move the player
            var position = (Vector2)transform.position;
            var positionVector = Vector2.MoveTowards(position, NextDestination, speed * Time.deltaTime);
            _rigidbody2D.MovePosition(positionVector);

            // Check if the player is centered in the tile
            var isCentered = position == NextDestination;
            if (!isCentered) return;

            // if is at the middle of a tile, has input and there is no wall in the direction of the input
            if (_lastInputDirection != Vector2.zero && !DetectWallBorder(_lastInputDirection))
            {
                NextDestination = position + _lastInputDirection;
                _lastDirection = _lastInputDirection;
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
            // Detect a tile in the direction of the dir vector parameter
            var pos = (Vector2)transform.position;
            var cellPosition = tilemap.WorldToCell(pos + dir);
            // Detect a door in the direction of the dir vector parameter using linecast
            var linecast = Physics2D.LinecastAll(pos + dir, pos);
            return linecast.Any(t => t.collider.CompareTag(tilemap.tag)) || tilemap.HasTile(cellPosition);
        }

        private void OnMove(InputValue value)
        {
            _inputDirection = value.Get<Vector2>();

            if (_inputDirection.x != 0) _inputDirection.y = 0; // Create a priority for x movement

            if (_inputDirection != Vector2.zero)
                _lastInputDirection =
                    _inputDirection.normalized; // Normalize the output to be 1 or -1 not floating values
        }

        private void OnPause()
        {
            GameHandler.GameHandler.Instance.ToglePause();
            MusicHandler.MusicHandler.Instance.TogglePause();
        }

        public void Reset()
        {
            transform.position = _spawnPosition;
            NextDestination = _spawnPosition + initDirection;
            _lastInputDirection = initDirection.normalized;
            _lastDirection = initDirection.normalized;
            RotateRenderer();
        }

        public void Immobilize()
        {
            _lastInputDirection = Vector2.zero;
            _lastDirection = Vector2.zero;
            NextDestination = transform.position;
        }
    }
}
