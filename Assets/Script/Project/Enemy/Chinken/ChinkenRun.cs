using UnityEngine;

namespace RiverCrab
{
    public class ChinkenRun : StateMachineBehaviour
    {
        ChinkenBehaviour CB;
        EnemyChinken EC;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo animInfo, int layerIndex)
        {
            CB = animator.GetComponent<ChinkenBehaviour>();
            EC = animator.GetComponent<EnemyChinken>();
        }

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
                animator.SetBool("Egg", true);                               
            }
            else
            {
                CB.Flip();

                if (CB.Patrol())
                {
                    animator.SetBool("Run", false);
                }
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger("Death");
            animator.ResetTrigger("Hit");
        }
    }
}
