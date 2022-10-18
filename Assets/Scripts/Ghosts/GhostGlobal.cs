using UnityEngine;

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
    }
}
