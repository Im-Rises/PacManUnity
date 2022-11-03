namespace Ghosts
{
    public class RedGhostAiMovement : GhostAiMovement
    {
        protected override void Chase()
        {
            ChaseTarget(chaseModeTarget, runSpeed);
        }
    }
}
