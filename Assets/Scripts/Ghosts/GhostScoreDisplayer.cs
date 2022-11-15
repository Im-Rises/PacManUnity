using UnityEngine;

namespace Ghosts
{
    public class GhostScoreDisplayer : MonoBehaviour
    {
        private SpriteRenderer _ghostScoreSpriteRenderer;
        public Sprite[] ghostScoreSpriteArray;

        private void Start()
        {
            _ghostScoreSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void DisplayGhostScore()
        {
            _ghostScoreSpriteRenderer.sprite = ghostScoreSpriteArray[GameHandler.GameHandler.Instance.GhostCountEaten];
        }

        public void HideGhostScore()
        {
            _ghostScoreSpriteRenderer.sprite = null;
        }
    }
}
