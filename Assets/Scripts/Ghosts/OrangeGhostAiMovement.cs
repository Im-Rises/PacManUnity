using UnityEngine;

namespace Ghosts
{
    public class OrangeGhostAiMovement : GhostAiMovement
    {
        // private bool _isScared; // Once the ghost is scared it switches to scatter mode for an amount of time

        // public float scatterTime = 7f;
        // private float _scatterTimer;

        protected override void Chase()
        {
            ChaseTarget(
                Vector2.Distance(transform.position, chaseModeTarget.transform.position) >= 8f
                    ? chaseModeTarget
                    : scatterModeTarget, runSpeed);
        }

        // protected override void Chase()
        // {
        //     if (!_isScared)
        //     {
        //         if (Vector2.Distance(transform.position, chaseModeTarget.transform.position) > 8f)
        //         {
        //             ChaseTarget(chaseModeTarget, runSpeed);
        //         }
        //         else
        //         {
        //             SetGhostMode(GhostMode.Scatter);
        //             _isScared = true;
        //         }
        //     }
        //     else
        //     {
        //         _scatterTimer += Time.deltaTime;
        //         if (_scatterTimer >= scatterTime)
        //         {
        //             _isScared = false;
        //             _scatterTimer = 0f;
        //         }
        //         else
        //         {
        //             ChaseTarget(scatterModeTarget, runSpeed);
        //         }
        //     }
        // }
    }
}
