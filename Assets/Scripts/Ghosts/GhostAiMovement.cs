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
        Eaten
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

        // // Ghost home
        // private bool _isInGhostHome;
        public Transform[] homeWayPoints;
        private bool _ghostHomeReached;
        private int _currentHomeWayPointIndex;
        private bool _isLeavingGhostHome;

        private void Start()
        {
            // Get components
            _rigidbody2D = GetComponent<Rigidbody2D>();

            // Set initial direction
            _direction = initDirection;
            NextTileDestination = (Vector2)transform.position + initDirection;
        }

        private void FixedUpdate()
        {
            switch (_ghostMode)
            {
                case GhostMode.Scatter:
                    Scatter();
                    UpdateRunAnimation();
                    break;
                case GhostMode.Chase:
                    Chase();
                    UpdateRunAnimation();
                    break;
                case GhostMode.Frightened:
                    Frightened();
                    break;
                case GhostMode.Eaten:
                    Eaten();
                    break;
                default:
                    Debug.LogError("Invalid Ghost Mode at 'FixedUpdate'");
                    break;
            }
        }

        public void SetGhostMode(GhostMode ghostMode)
        {
            if (_ghostMode == GhostMode.Eaten && !_isLeavingGhostHome)
                return; // If ghost is eaten, it can't change mode until it reaches the ghost home (except if it's leaving the ghost home)

            _ghostMode = ghostMode;

            switch (_ghostMode)
            {
                case GhostMode.Scatter:
                case GhostMode.Chase:
                    bodyRenderer.enabled = true;
                    UpdateRunAnimation();
                    break;
                case GhostMode.Frightened:
                    bodyRenderer.enabled = true;
                    eyesSpriteRenderer.sprite = eyesSpriteArray[4];
                    UpdateFrightenedAnimation();
                    break;
                case GhostMode.Eaten:
                    bodyRenderer.enabled = false;
                    UpdateEatenAnimation();
                    break;
                default:
                    // Debug.LogError("Invalid Ghost Mode at 'SetGhostMode'");
                    throw new ArgumentOutOfRangeException();
            }

            _hasChangedMode = true;
        }

        #region Ghost Modes

        // Ghost movement methods

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
            var pos = transform.position;
            var waypoint = homeWayPoints[0].position;

            // Check if ghost has reached the ghost home
            if (!_ghostHomeReached)
            {
                if (Vector2.Distance(pos, waypoint) > 1f)
                {
                    ChaseTarget(ghostHomeEntry, eatenSpeed);
                }
                else
                {
                    _ghostHomeReached = true;
                    bodyRenderer.enabled = true;
                    UpdateRunAnimation();
                }

                // UpdateEatenAnimation();
            }
            // if the ghost has reached the ghost home
            else
            {
                // Follow the ghost home waypoints
                if (!_isLeavingGhostHome)
                {
                    _isLeavingGhostHome = FollowPath(eatenSpeed);
                }
                else
                {
                    // Reverse the ghost home waypoints
                    var hasLeavedHouse = FollowPath(runSpeed, true);

                    // If the ghost has left the ghost home, then set the ghost mode to the current Game Ghost Mode
                    if (hasLeavedHouse)
                    {
                        SetGhostMode(GameHandler.GameHandler.Instance.GameGhostsMode);
                        _ghostHomeReached = false;
                        _isLeavingGhostHome = false;
                        _currentHomeWayPointIndex = 0;
                    }
                }
            }
        }

        private bool FollowPath(float speed, bool reverse = false)
        {
            if (transform.position != homeWayPoints[_currentHomeWayPointIndex].position)
            {
                var p = Vector2.MoveTowards(transform.position,
                    homeWayPoints[_currentHomeWayPointIndex].position,
                    speed * Time.deltaTime);
                _rigidbody2D.MovePosition(p);
                UpdateFollowPathAnimation();
            }
            else
            {
                switch (reverse)
                {
                    case false when _currentHomeWayPointIndex == homeWayPoints.Length - 1:
                    case true when _currentHomeWayPointIndex == 0:
                        return true;
                    default:
                        _currentHomeWayPointIndex += reverse ? -1 : 1;
                        break;
                }
            }

            return false;
        }

        #endregion

        #region Chase target function

        // Chase sub-functions

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

            List<Vector2> possibleDirections;
            // Check where the ghost can go
            // possibleDirections = _ghostMode == GhostMode.Eaten
            //     ? FindPossibleDirectionsWithoutDoor()
            //     : FindPossibleDirections();
            possibleDirections = FindPossibleDirections();

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
            if (!DetectWallsAndDoor(up))
                possibleDirections.Add(up);
            if (!DetectWallsAndDoor(down))
                possibleDirections.Add(down);
            if (!DetectWallsAndDoor(left))
                possibleDirections.Add(left);
            if (!DetectWallsAndDoor(right))
                possibleDirections.Add(right);

            return possibleDirections;
        }

        // private List<Vector2> FindPossibleDirectionsWithoutDoor()
        // {
        //     var up = Vector2.up;
        //     var down = Vector2.down;
        //     var left = Vector2.left;
        //     var right = Vector2.right;
        //
        //     var pos = transform.position;
        //
        //     var possibleDirections = new List<Vector2>();
        //     if (!DetectWalls(up, pos))
        //         possibleDirections.Add(up);
        //     if (!DetectWalls(down, pos))
        //         possibleDirections.Add(down);
        //     if (!DetectWalls(left, pos))
        //         possibleDirections.Add(left);
        //     if (!DetectWalls(right, pos))
        //         possibleDirections.Add(right);
        //
        //     return possibleDirections;
        // }

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

        private bool DetectWallsAndDoor(Vector2 dir)
        {
            var pos = (Vector2)transform.position;
            return DetectWalls(dir, pos) || DetectDoor(dir, pos);
        }

        private bool DetectWalls(Vector2 dir, Vector2 pos)
        {
            // Detect a wall or border using grid's tiles
            var cellPosition = tilemap.WorldToCell(pos + dir);
            return tilemap.HasTile(cellPosition);
        }

        private bool DetectDoor(Vector2 dir, Vector2 pos)
        {
            // Detect the door using linecast and tags
            var linecast = Physics2D.Linecast(pos + dir, pos);
            return linecast.collider.CompareTag(tilemap.tag);
        }

        #endregion

        #region Animations functions

        // Animations methods
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

        private void UpdateFrightenedAnimation()
        {
            eyesSpriteRenderer.sprite = eyesSpriteArray[4];
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

        private void UpdateFollowPathAnimation()
        {
            Vector2 dir = homeWayPoints[_currentHomeWayPointIndex].position - transform.position;
            eyesSpriteRenderer.sprite = dir.y switch
            {
                > 0 => eyesSpriteArray[2],
                < 0 => eyesSpriteArray[3],
                _ => dir.x switch
                {
                    > 0 => eyesSpriteArray[0],
                    < 0 => eyesSpriteArray[1],
                    _ => eyesSpriteRenderer.sprite
                }
            };
        }

        #endregion

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagsConstants.PlayerTag))
                if (_ghostMode is GhostMode.Frightened or GhostMode.Eaten)
                    SetGhostMode(GhostMode.Eaten);
                else
                    GameHandler.GameHandler.Instance.KillPlayer();
        }
    }
}
