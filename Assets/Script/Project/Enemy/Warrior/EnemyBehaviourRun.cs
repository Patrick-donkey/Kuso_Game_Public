using UnityEngine;

namespace RiverCrab
{
    public class WarriorBehaviorRun : StateMachineBehaviour
    {
        WarriorBehaviour WB;
        EnemyWarrior EW;
        
        override public void OnStateEnter(Animator animator, AnimatorStateInfo animInfo, int layerIndex)
        {
            WB = animator.GetComponent<WarriorBehaviour>();
            EW = animator.GetComponent<EnemyWarrior>();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (EW.dead)
            {
                animator.SetTrigger("Death");
            }
            if(EW.hit)
            {
                animator.SetTrigger("Hit");
            }

            if (WB.PlayerFound())
            {
                if (WB.HeavyAttackFound())
                {
                    animator.SetBool("HeavyAttack", true);
                }

                else if (WB.AttackFound())
                {
                    animator.SetTrigger("Attack");
                }
            }
            else
            {
                WB.Flip();

                if (WB.Patrol())
                {
                    animator.SetBool("Run", false);
                }                           
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Death");
            animator.ResetTrigger("Hit");
        }
    }
}
