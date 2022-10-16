using System;
using Player;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ghosts
{
    public class GhostMovement : MonoBehaviour
    {
        public float speed = 4f;
        public Transform[] scatterModeWaypoints;
        public SpriteRenderer eyesSpriteRenderer;
        public Sprite[] eyesSpriteArray;
        public Vector2 originalDirection = new(-1, 0);
        public float initPositionOffset = 0.5f;
        public Tilemap tilemap;

        public GameObject chaseModeTarget;
        private readonly GhostGlobal.GhostMode _ghostMode = GhostGlobal.GhostMode.Chase;
        private Vector2 _direction;
        private Vector2 _nextDestination;
        private Rigidbody2D _rigidbody2D;
        private int _scatterModePosition;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            switch (_ghostMode)
            {
                case GhostGlobal.GhostMode.Scatter:
                    ScatterMode();
                    break;
                case GhostGlobal.GhostMode.Chase:
                    ChaseMode();
                    break;
                case GhostGlobal.GhostMode.Frightened:
                    FrightenedMode();
                    break;
                case GhostGlobal.GhostMode.Eaten:
                    EatenMode();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UpdateAnimation();
        }

        private void OnTriggerEnter2D(Collider2D co)
        {
            if (co.name == "Player")
                co.GetComponent<PlayerLife>().Die();
        }

        private void ChaseMode()
        {
            var position = transform.position;
            var positionVector = Vector2.MoveTowards(position, _nextDestination, speed * Time.deltaTime);
            _rigidbody2D.MovePosition(positionVector);

            var isCentered = (Vector2)position == _nextDestination;

            // if ghost is centered in a tile check all possible directions
            // and choose the one that leads to the player quickly using direct vector

            // if (!isCentered || GhostGlobal.DetectWallBorder(tilemap, _direction, transform.position)) return;
            // _nextDestination = (Vector2)transform.position + _direction;
        }

        private void FrightenedMode()
        {
        }

        private void EatenMode()
        {
        }

        private void ScatterMode()
        {
            if (transform.position != scatterModeWaypoints[_scatterModePosition].position)
            {
                var p = Vector2.MoveTowards(transform.position,
                    scatterModeWaypoints[_scatterModePosition].position,
                    speed * Time.deltaTime);
                _rigidbody2D.MovePosition(p);
            }
            else
            {
                _scatterModePosition = (_scatterModePosition + 1) % scatterModeWaypoints.Length;
            }
        }

        private void UpdateAnimation()
        {
            var dir = scatterModeWaypoints[_scatterModePosition].position - transform.position;
            if (dir.x > 0)
                eyesSpriteRenderer.sprite = eyesSpriteArray[0];
            if (dir.x < 0)
                eyesSpriteRenderer.sprite = eyesSpriteArray[1];
            if (dir.y > 0)
                eyesSpriteRenderer.sprite = eyesSpriteArray[2];
            if (dir.y < 0)
                eyesSpriteRenderer.sprite = eyesSpriteArray[3];
        }
    }
}
