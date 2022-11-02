using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DebugPacMan
{
    public class DebugWallBorders : MonoBehaviour
    {
        public Tilemap tilemap;

        private void FixedUpdate()
        {
            Debug.Log("---Wall Borders: " + transform.position + "---");
            Debug.Log("Wall Detected Up: " + DetectWallsAndDoors(Vector2.up));
            Debug.Log("Wall Detected Down: " + DetectWallsAndDoors(Vector2.down));
            Debug.Log("Wall Detected Left: " + DetectWallsAndDoors(Vector2.left));
            Debug.Log("Wall Detected Right: " + DetectWallsAndDoors(Vector2.right));
        }

        private bool DetectWallsAndDoors(Vector2 dir)
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
    }
}
