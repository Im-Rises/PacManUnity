using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Ghosts
{
    public enum GhostMode
    {
        Chase,
        Scatter,
        Frightened,
        Eaten,
        LeavingHouse
    }

    public abstract class GhostAiMovement : MonoBehaviour
    {
        // Movement speed variables
        public float runSpeed = 4f;
        public float frightenedSpeed = 1f;
        public float eatenSpeed = 20f;

        // Tilemap variables
        public Tilemap tilemap;

        // Ghost target variables
        public GameObject chaseModeTarget;
        public GameObject scatterModeTarget;
        public GameObject ghostHomeEntry;

        // Sprites variables
        public SpriteRenderer eyesSpriteRenderer;
        public Sprite[] eyesSpriteArray;
        public SpriteRenderer bodyRenderer;

        // Ghost direction variables
        public Vector2 initDirection = Vector2.left;
        private Vector2 _direction;

        // Ghost current mode
        private GhostMode _ghostMode;
        private bool _hasChangedMode;
        public Vector2 NextTileDestination { get; set; }

        // Components
        private Rigidbody2D _rigidbody2D;

        // Ghost home
        public bool isInGhostHouse;
        private bool _ghostHomeReached;
        public Transform[] enterHomeWayPoints;
        public Transform[] exitHomeWayPoints;
        private int _currentWayPointIndex;

        private void Start()
        {
            // Get components
            _rigidbody2D = GetComponent<Rigidbody2D>();

            // Set initial direction
            _direction = initDirection;
            NextTileDestination = (Vector2)transform.position + initDirection;

            // Set initial mode
            if (isInGhostHouse) _ghostMode = GhostMode.LeavingHouse;
        }

        private void FixedUpdate()
        {
            switch (_ghostMode)
            {
                case GhostMode.Scatter:
                    Scatter();
                    break;
                case GhostMode.Chase:
                    Chase();
                    break;
                case GhostMode.Frightened:
                    Frightened();
                    break;
                case GhostMode.Eaten:
                    Eaten();
                    break;
                case GhostMode.LeavingHouse:
                    LeavingHouse();
                    break;
                default:
                    // Debug.LogError("Invalid Ghost Mode at 'FixedUpdate'");
                    break;
            }
        }

        public void SetGhostMode(GhostMode ghostMode, bool forceChange = false)
        {
            if (_ghostMode is GhostMode.Eaten or GhostMode.LeavingHouse && forceChange is false)
                return; // If ghost is eaten or is leaving home, it can't change mode.

            _ghostMode = ghostMode;

            switch (_ghostMode)
            {
                case GhostMode.Scatter:
                case GhostMode.Chase:
                    UpdateRunAnimation();
                    break;
                case GhostMode.Frightened:
                    UpdateFrightenedAnimation();
                    break;
                case GhostMode.Eaten:
                    bodyRenderer.enabled = false;
                    UpdateEatenAnimation();
                    break;
                case GhostMode.LeavingHouse:
                    bodyRenderer.enabled = true;
                    break;
                default:
                    // Debug.LogError("Invalid Ghost Mode at 'SetGhostMode'");
                    throw new ArgumentOutOfRangeException();
            }

            _hasChangedMode = true;
        }

        #region Ghost Modes

        protected virtual void Chase()
        {
            // Different depending on ghost
        }

        private void Scatter()
        {
            ChaseTarget(scatterModeTarget, runSpeed);
        }

        private void Frightened()
        {
            var position = (Vector2)transform.position;

            // Move ghost
            MoveGhost(position, frightenedSpeed);

            // Check if ghost reached next tile
            var isCentered = position == NextTileDestination;
            if (!isCentered)
                return;

            if (_hasChangedMode)
            {
                _direction *= -1;
                _hasChangedMode = false;
            }
            else
            {
                // Check where the ghost can go
                var possibleDirections = FindPossibleDirections();

                // if two or more possible directions then delete the opposite direction (preventing the ghost from going back)
                if (possibleDirections.Count > 1)
                    for (var i = 0; i < possibleDirections.Count; i++)
                        if (possibleDirections[i] == -_direction)
                            possibleDirections.RemoveAt(i);

                // Choose a random direction
                var randomDirection = possibleDirections[Random.Range(0, possibleDirections.Count)];

                // Set new direction
                _direction = randomDirection;
            }

            NextTileDestination = position + _direction;
        }

        private void Eaten()
        {
            if (!_ghostHomeReached)
            {
                ChaseTarget((Vector2)enterHomeWayPoints[0].transform.position, eatenSpeed);
                if (Vector2.Distance(transform.position, enterHomeWayPoints[0].transform.position) <= 0.5f)
                    _ghostHomeReached = true;

                UpdateEatenAnimation();
                return;
            }
            else if (FollowPath(enterHomeWayPoints, ref _currentWayPointIndex, eatenSpeed))
            {
                _ghostHomeReached = false;
                SetGhostMode(GhostMode.LeavingHouse, true);
            }

            UpdateFollowPathEatenAnimation(enterHomeWayPoints, _currentWayPointIndex);
        }

        private void LeavingHouse()
        {
            UpdateFollowPathRunAnimation(enterHomeWayPoints, _currentWayPointIndex);
            if (FollowPath(exitHomeWayPoints, ref _currentWayPointIndex, runSpeed))
                SetGhostMode(GameHandler.GameHandler.Instance.GameGhostsMode, true);
        }

        #endregion

        #region Chase target function

        private bool FollowPath(Transform[] waypoints, ref int currentWaypoint, float speed)
        {
            if (transform.position != waypoints[currentWaypoint].position)
            {
                var p = Vector2.MoveTowards(transform.position,
                    waypoints[currentWaypoint].position,
                    speed * Time.deltaTime);
                _rigidbody2D.MovePosition(p);
            }
            else
            {
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Length)
                {
                    _currentWayPointIndex = 0;
                    return true;
                }
            }

            return false;
        }

        protected void ChaseTarget(GameObject target, float speed)
        {
            ChaseTarget(target.transform.position, speed);
        }

        protected void ChaseTarget(Vector2 targetPos, float speed)
        {
            var position = (Vector2)transform.position;

            // Move ghost
            MoveGhost(position, speed);

            // Check if ghost reached next tile
            var isCentered = position == NextTileDestination;
            if (!isCentered)
                return;

            var possibleDirections = FindPossibleDirections();

            // if two or more possible directions then delete the opposite direction (preventing the ghost from going back)
            if (possibleDirections.Count > 1)
                for (var i = 0; i < possibleDirections.Count; i++)
                    if (possibleDirections[i] == -_direction)
                        possibleDirections.RemoveAt(i);

            // Calculate the shortest distance to the target
            CalculateNextTileDestination(possibleDirections, position, targetPos);

            // Update animation each tile
            switch (_ghostMode)
            {
                case GhostMode.Chase or GhostMode.Scatter:
                    UpdateRunAnimation();
                    break;
                case GhostMode.Eaten:
                    UpdateEatenAnimation();
                    break;
            }
        }

        #endregion

        #region Find possible directions

        private List<Vector2> FindPossibleDirections()
        {
            var up = Vector2.up;
            var down = Vector2.down;
            var left = Vector2.left;
            var right = Vector2.right;

            var possibleDirections = new List<Vector2>();
            if (!DetectWallsAndDoors(up))
                possibleDirections.Add(up);
            if (!DetectWallsAndDoors(down))
                possibleDirections.Add(down);
            if (!DetectWallsAndDoors(left))
                possibleDirections.Add(left);
            if (!DetectWallsAndDoors(right))
                possibleDirections.Add(right);

            return possibleDirections;
        }

        private void CalculateNextTileDestination(List<Vector2> possibleDirections, Vector2 position, Vector2 targetPos)
        {
            var shortestDistance = float.MaxValue;
            var shortestDirection = Vector2.zero;

            foreach (var direction in possibleDirections)
            {
                var distance = Vector2.Distance(targetPos, position + direction);

                if (!(distance < shortestDistance)) continue;

                shortestDistance = distance;
                shortestDirection = direction;
            }

            _direction = shortestDirection;
            NextTileDestination = position + _direction;
        }

        #endregion

        #region Movements sub-functions

        private void MoveGhost(Vector2 position, float speed)
        {
            var positionVector = Vector2.MoveTowards(position, NextTileDestination, speed * Time.deltaTime);
            _rigidbody2D.MovePosition(positionVector);
        }

        private bool DetectWallsAndDoors(Vector2 dir)
        {
            var pos = (Vector2)transform.position;
            return DetectWalls(dir, pos) || DetectDoor(dir, pos);
        }

        private bool DetectWalls(Vector2 dir, Vector2 pos)
        {
            var cellPosition = tilemap.WorldToCell(pos + dir);
            return tilemap.HasTile(cellPosition);
        }

        private bool DetectDoor(Vector2 dir, Vector2 pos)
        {
            var linecast = Physics2D.Linecast(pos + dir * 1.2f, pos); // offset the linecast to detect the door
            return linecast.collider.CompareTag(tilemap.tag);
        }

        #endregion

        #region Animations functions

        private void UpdateFrightenedAnimation()
        {
            eyesSpriteRenderer.sprite = eyesSpriteArray[4];
        }

        private void UpdateRunAnimation()
        {
            eyesSpriteRenderer.sprite = _direction.y switch
            {
                > 0 => eyesSpriteArray[2],
                < 0 => eyesSpriteArray[3],
                _ => _direction.x switch
                {
                    > 0 => eyesSpriteArray[0],
                    < 0 => eyesSpriteArray[1],
                    _ => eyesSpriteRenderer.sprite
                }
            };
        }

        private void UpdateEatenAnimation()
        {
            eyesSpriteRenderer.sprite = _direction.y switch
            {
                > 0 => eyesSpriteArray[7],
                < 0 => eyesSpriteArray[8],
                _ => _direction.x switch
                {
                    > 0 => eyesSpriteArray[5],
                    < 0 => eyesSpriteArray[6],
                    _ => eyesSpriteRenderer.sprite
                }
            };
        }

        private void UpdateFollowPathRunAnimation(Transform[] waypoints, int currentWaypoint)
        {
            // Vector2 dir = waypoints[currentWaypoint].position - transform.position;
            // eyesSpriteRenderer.sprite = dir.y switch
            // {
            //     > 0 => eyesSpriteArray[2],
            //     < 0 => eyesSpriteArray[3],
            //     _ => dir.x switch
            //     {
            //         > 0 => eyesSpriteArray[0],
            //         < 0 => eyesSpriteArray[1],
            //         _ => eyesSpriteRenderer.sprite
            //     }
            // };
        }

        private void UpdateFollowPathEatenAnimation(Transform[] waypoints, int currentWaypoint)
        {
            Vector2 dir = waypoints[currentWaypoint].position - transform.position;
            eyesSpriteRenderer.sprite = dir.y switch
            {
                > 0 => eyesSpriteArray[7],
                < 0 => eyesSpriteArray[8],
                _ => dir.x switch
                {
                    > 0 => eyesSpriteArray[5],
                    < 0 => eyesSpriteArray[6],
                    _ => eyesSpriteRenderer.sprite
                }
            };
        }

        #endregion

        #region On trigger functions

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagsConstants.PlayerTag))
                if (_ghostMode is GhostMode.Frightened or GhostMode.Eaten)
                    SetGhostMode(GhostMode.Eaten);
                else
                    GameHandler.GameHandler.Instance.KillPlayer();
        }

        #endregion
    }
}
