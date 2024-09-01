using UnityEngine;

namespace RiverCrab
{
    public class Canine_Idle : StateMachineBehaviour
    {
        CanineBehaviour EB;
        EnemyCanine EC;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            EB = animator.GetComponent<CanineBehaviour>();
            EC = animator.GetComponent<EnemyCanine>();
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
