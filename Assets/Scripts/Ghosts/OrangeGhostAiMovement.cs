using UnityEngine;

namespace Ghosts
{
    public class OrangeGhostAiMovement : GhostAiMovement
    {
        private bool _isScared; // Once the ghost is scared it switches to scatter mode for an amount of time

        public float scatterTime = 7f;
        private float _scatterTimer;


        protected override void Chase()
        {
            if (Vector2.Distance(transform.position, chaseModeTarget.transform.position) > 8f)
                ChaseTarget(chaseModeTarget, runSpeed);
            else
                SetGhostMode(GhostMode.Scatter);
        }
    }
}
