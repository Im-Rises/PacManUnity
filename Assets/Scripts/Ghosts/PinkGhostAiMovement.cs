namespace Ghosts
{
    public class PinkGhostAiMovement : GhostAiMovement
    {
        protected override void Chase()
        {
            ChaseTarget(chaseModeTarget, runSpeed);
        }
    }
}
