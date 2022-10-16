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
            // var positionVector = Vector2.MoveTowards(position, _nextTileDestination, speed * Time.deltaTime);
            // _rigidbody2D.MovePosition(positionVector);


            Debug.Log("Up tile " + (position + Vector2.up) +
                      (DetectWallBorder(position + Vector2.up) ? " Wall" : "Empty"));
            Debug.Log("Right tile " + (position + Vector2.right) +
                      (DetectWallBorder(position + Vector2.right) ? " Wall" : "Empty"));
            Debug.Log("Left tile " + (position + Vector2.left) +
                      (DetectWallBorder(position + Vector2.left) ? " Wall" : "Empty"));
            Debug.Log("Down tile " + (position + Vector2.down) +
                      (DetectWallBorder(position + Vector2.down) ? " Wall" : "Empty"));


            var isCentered = position == _nextTileDestination;
            if (!isCentered) return;

            var possibleDirections = new Vector2[4];
            var possibleDirectionsCount = 0;

            var up = Vector2.up;
            var down = Vector2.down;
            var left = Vector2.left;
            var right = Vector2.right;

            if (!DetectWallBorder(position + up))
                possibleDirections[possibleDirectionsCount++] = up;
            if (!DetectWallBorder(position + down))
                possibleDirections[possibleDirectionsCount++] = down;
            if (!DetectWallBorder(position + left))
                possibleDirections[possibleDirectionsCount++] = left;
            if (!DetectWallBorder(position + right))
                possibleDirections[possibleDirectionsCount++] = right;

            var shortestDistance = float.MaxValue;
            var shortestDirection = Vector2.zero;

            // Debug.Log(possibleDirectionsCount + " " + possibleDirections[0] + " " + possibleDirections[1] + " " +
            //           possibleDirections[2] + " " + possibleDirections[3]);
            // Debug.Log("Up tile " + " Position: " + (position + up) +
            //           (DetectWallBorder(position + up) ? " Wall" : "Empty"));

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
            var cellPosition = tilemap.WorldToCell(pos + dir);
            // var linecast = Physics2D.Linecast(pos + dir, pos);
            // Debug.Log(tilemap.HasTile(cellPosition));
            return tilemap.HasTile(cellPosition);
            // return tilemap.HasTile(cellPosition) || linecast.collider.CompareTag(tilemap.tag);
        }
    }
}
