using UnityEngine;

namespace RiverCrab
{
    public class EnemyBehaviourIdle : StateMachineBehaviour
    {
        WarriorBehaviour EB;
        EnemyWarrior EW;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            EB = animator.GetComponent<WarriorBehaviour>();
            EW = animator.GetComponent<EnemyWarrior>();
        }

        //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (EW.dead)
            {
                animator.SetTrigger("Death");
            }
            if (EW.hit)
            {
                animator.SetTrigger("Hit");
            }

            if (EB.PlayerFound())
            {
                animator.SetBool("Run", true);
            }
            if (EB.SwitchToRun())
            {
                animator.SetBool("Run", true);
            }
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger("Death");
            animator.ResetTrigger("Hit");
        }
    }
}
