using System;
using System.Linq;
using GameHandler;
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
        public Vector2 LastInputDirection { get; set; }
        private Vector2 _lastDirection;

        // Sprite
        public SpriteRenderer spriteRenderer;


        public Vector2 NextDestination { get; set; }

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            var position = transform.position;
            _spawnPosition = position;
            _lastDirection = initDirection.normalized;
            NextDestination = (Vector2)position + _lastDirection / 2;
            RotateRenderer();
        }

        private void FixedUpdate()
        {
            Debug.Log("FixedUpdate " + LastInputDirection);

            // Move the player
            var position = (Vector2)transform.position;
            var positionVector = Vector2.MoveTowards(position, NextDestination, speed * Time.deltaTime);
            _rigidbody2D.MovePosition(positionVector);

            // Check if the player is centered in the tile
            var isCentered = position == NextDestination;
            if (!isCentered) return;

            // if is at the middle of a tile, has input and there is no wall in the direction of the input
            if (LastInputDirection != Vector2.zero && !DetectWallBorder(LastInputDirection))
            {
                NextDestination = position + LastInputDirection;
                _lastDirection = LastInputDirection;
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
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, angle);
            // transform.rotation = Quaternion.Euler(0, 0, angle);
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

        public void Reset()
        {
            transform.position = _spawnPosition;
            NextDestination = _spawnPosition + initDirection;
            LastInputDirection = initDirection.normalized;
            _lastDirection = initDirection.normalized;
            RotateRenderer();
        }

        public void Immobilize()
        {
            LastInputDirection = Vector2.zero;
            _lastDirection = Vector2.zero;
            NextDestination = transform.position;
        }
    }
}
