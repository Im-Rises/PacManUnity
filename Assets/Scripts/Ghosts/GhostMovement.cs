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
        private Vector2 _nextTileDestination;
        private Rigidbody2D _rigidbody2D;
        private int _scatterModePosition;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _nextTileDestination = (Vector2)transform.position + originalDirection * initPositionOffset;
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
            var position = (Vector2)transform.position;
            var positionVector = Vector2.MoveTowards(position, _nextTileDestination, speed * Time.deltaTime);
            _rigidbody2D.MovePosition(positionVector);


            var isCentered = position == _nextTileDestination;
            if (!isCentered) return;

            var possibleDirections = new Vector2[4];
            var possibleDirectionsCount = 0;

            var up = Vector2.up;
            var down = Vector2.down;
            var left = Vector2.left;
            var right = Vector2.right;

            if (!DetectWallBorder(up))
                possibleDirections[possibleDirectionsCount++] = up;
            if (!DetectWallBorder(down))
                possibleDirections[possibleDirectionsCount++] = down;
            if (!DetectWallBorder(left))
                possibleDirections[possibleDirectionsCount++] = left;
            if (!DetectWallBorder(right))
                possibleDirections[possibleDirectionsCount++] = right;

            if (possibleDirectionsCount > 2)
            {
                var shortestDistance = float.MaxValue;
                var shortestDirection = Vector2.zero;

                for (var i = 0; i < possibleDirectionsCount; i++)
                {
                    var direction = possibleDirections[i];
                    var distance = Vector2.Distance(chaseModeTarget.transform.position, position + direction);

                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        shortestDirection = direction;
                    }
                }

                _direction = shortestDirection;
                _nextTileDestination = position + _direction;
            }
            else if (possibleDirectionsCount == 2)
            {
                if (possibleDirections[0] == _direction * -1)
                    _direction = possibleDirections[1];
                else
                    _direction = possibleDirections[0];

                _nextTileDestination = position + _direction;
            }
            else if (possibleDirectionsCount == 1)
            {
                _direction = possibleDirections[0];
                _nextTileDestination = position + _direction;
            }
            else
            {
                _direction = _direction * -1;
                _nextTileDestination = position + _direction;
            }
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
            // var dir = scatterModeWaypoints[_scatterModePosition].position - transform.position;
            // if (dir.x > 0)
            //     eyesSpriteRenderer.sprite = eyesSpriteArray[0];
            // if (dir.x < 0)
            //     eyesSpriteRenderer.sprite = eyesSpriteArray[1];
            // if (dir.y > 0)
            //     eyesSpriteRenderer.sprite = eyesSpriteArray[2];
            // if (dir.y < 0)
            //     eyesSpriteRenderer.sprite = eyesSpriteArray[3];
        }

        private bool DetectWallBorder(Vector2 dir)
        {
            var pos = (Vector2)transform.position;
            var cellPosition = tilemap.WorldToCell(pos + dir); // Detect a wall or border with using grid's tiles
            var linecast = Physics2D.Linecast(pos + dir, pos); // Detect a wall or border using linecast and tags
            return tilemap.HasTile(cellPosition) || linecast.collider.CompareTag(tilemap.tag);
        }
    }
}
