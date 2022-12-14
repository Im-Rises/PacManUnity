using UnityEngine;

namespace Collectibles
{
    public class CollectiblePower : MonoBehaviour
    {
        public int points = 100;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag(TagsConstants.PlayerTag)) return;
            ScoreHandler.ScoreHandler.Instance.AddScore(points);
            GameHandler.GameHandler.Instance.SwitchingFrightenedMode();
            Destroy(gameObject);
        }
    }
}
