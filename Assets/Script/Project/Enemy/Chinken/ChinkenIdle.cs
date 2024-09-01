using UnityEngine;

namespace RiverCrab
{
    public class ChinkenIdle : StateMachineBehaviour
    {
        ChinkenBehaviour CB;
        EnemyChinken EC;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CB = animator.GetComponent<ChinkenBehaviour>();
            EC = animator.GetComponent<EnemyChinken>();
        }

        //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (EC.dead)
            {
                animator.SetTrigger("Death");
            }

            if (EC.hit)
            {
                animator.SetTrigger("Hit");
            }

            if (CB.PlayerFound())
            {
                animator.SetBool("Run", true);
            }
            if (CB.SwitchToRun())
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
