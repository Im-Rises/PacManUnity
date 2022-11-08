using UnityEngine;

namespace MusicHandler
{
    public class MusicHandler : MonoBehaviour
    {
        public static MusicHandler Instance { get; private set; }

        public AudioSource ghostChase;
        public AudioSource pacmanChase;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        public void PlayGhostChase()
        {
            ghostChase.Play();
            ghostChase.loop = true;

            pacmanChase.Stop();
        }

        public void PlayPacmanChase()
        {
            pacmanChase.Play();
            pacmanChase.loop = true;

            ghostChase.Stop();
        }


        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}
