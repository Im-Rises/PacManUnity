using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ghosts
{
    public class GhostGlobal : MonoBehaviour
    {
        public enum GhostMode
        {
            Scatter = 0,
            Chase = 1,
            Frightened = 2,
            Eaten = 3
        }

        public static bool DetectWallBorder(Tilemap tilemap, Vector3 dir, Vector3 pos)
        {
            var cellPosition = tilemap.WorldToCell(pos + dir);
            var linecast = Physics2D.Linecast(pos + dir, pos);
            return tilemap.HasTile(cellPosition) || linecast.collider.CompareTag(tilemap.tag);
        }
    }
}
