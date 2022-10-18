using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ghosts
{
    public enum GhostMode
    {
        Scatter = 0,
        Chase = 1,
        Frightened = 2,
        Eaten = 3
    }

    public class GhostAiMovement : MonoBehaviour
    {
        // Movement speed variables
        public float runSpeed = 4f;
        public float frightenedSpeed = 1f;
        public float eatenSpeed = 20f;

        // Sprites variables
        public SpriteRenderer eyesSpriteRenderer;
        public Sprite[] eyesSpriteArray;
        public SpriteRenderer bodyRenderer;

        // Ghost movement variables
        public Tilemap tilemap;
        public GameObject ghostHome;
        public GameObject scatterModeTarget;
        public GameObject ghostHomeEntry;
        public GameObject chaseModeTarget;

        // Offset init position
        public float initPositionOffset = 0.5f;
        public Vector2 originalDirection = new(-1, 0);

        // Ghost home variables
        public bool isInGhostHouse;

        // Ghost direction variables
        private Vector2 _direction;
        private Vector2 _nextTileDestination;

        // Components
        private Rigidbody2D _rigidbody2D;

        public GhostMode GhostMode { get; set; }

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _nextTileDestination = (Vector2)transform.position + originalDirection * initPositionOffset;
        }

        private void FixedUpdate()
        {
            switch (GhostMode)
            {
                case GhostMode.Scatter:
                    Chase(scatterModeTarget, runSpeed);
                    UpdateRunAnimation();
                    break;
                case GhostMode.Chase:
                    Chase(chaseModeTarget, runSpeed);
                    UpdateRunAnimation();
                    break;
                case GhostMode.Frightened:
                    Frightened();
                    break;
                case GhostMode.Eaten:
                    Eaten();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Chase(GameObject target, float speed)
        {
            var position = (Vector2)transform.position;

            // Move ghost
            MoveGhost(position, speed);

            // Check if ghost reached next tile
            var isCentered = position == _nextTileDestination;
            if (!isCentered) return;

            // Check where the ghost can go
            var up = Vector2.up;
            var down = Vector2.down;
            var left = Vector2.left;
            var right = Vector2.right;

            var possibleDirections = new List<Vector2>();

            if (!DetectWallBorder(up))
                possibleDirections.Add(up);
            if (!DetectWallBorder(down))
                possibleDirections.Add(down);
            if (!DetectWallBorder(left))
                possibleDirections.Add(left);
            if (!DetectWallBorder(right))
                possibleDirections.Add(right);

            // if two or more possible directions then delete the opposite direction (preventing the ghost from going back)
            if (possibleDirections.Count > 1)
                for (var i = 0; i < possibleDirections.Count; i++)
                    if (possibleDirections[i] == -_direction)
                        possibleDirections.RemoveAt(i);


            // Calculate the shortest distance to the target
            var shortestDistance = float.MaxValue;
            var shortestDirection = Vector2.zero;

            foreach (var direction in possibleDirections)
            {
                var distance = Vector2.Distance(target.transform.position, position + direction);

                if (!(distance < shortestDistance)) continue;

                shortestDistance = distance;
                shortestDirection = direction;
            }

            _direction = shortestDirection;
            _nextTileDestination = position + _direction;
        }

        private void Frightened()
        {
            eyesSpriteRenderer.sprite = eyesSpriteArray[4];
            Chase(scatterModeTarget, frightenedSpeed);
        }

        private void Eaten()
        {
            if (!isInGhostHouse)
            {
                bodyRenderer.enabled = false;
                Chase(ghostHome, eatenSpeed);
                isInGhostHouse = ghostHome.transform.position == transform.position;
                UpdateFrightenedAnimation();
            }
            else
            {
                Chase(ghostHomeEntry, eatenSpeed);
                if (ghostHomeEntry.transform.position == transform.position)
                {
                    isInGhostHouse = false;
                    // GhostMode = GhostMode.Scatter;
                    bodyRenderer.enabled = true;
                }

                UpdateRunAnimation();
            }
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

        private void UpdateFrightenedAnimation()
        {
            eyesSpriteRenderer.sprite = _direction.y switch
            {
                > 0 => eyesSpriteArray[5],
                < 0 => eyesSpriteArray[6],
                _ => _direction.x switch
                {
                    > 0 => eyesSpriteArray[7],
                    < 0 => eyesSpriteArray[8],
                    _ => eyesSpriteRenderer.sprite
                }
            };
        }

        private void MoveGhost(Vector2 position, float speed)
        {
            var positionVector = Vector2.MoveTowards(position, _nextTileDestination, speed * Time.deltaTime);
            _rigidbody2D.MovePosition(positionVector);
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
