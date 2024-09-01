using UnityEngine;

namespace RiverCrab
{
    public class Canine_Run : StateMachineBehaviour
    {
        CanineBehaviour enemyBehavior;
        EnemyCanine EC;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo animInfo, int layerIndex)
        {
            enemyBehavior = animator.GetComponent<CanineBehaviour>();
            EC = animator.GetComponent<EnemyCanine>();
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

            if (enemyBehavior.PlayerFound())
            {
                if (enemyBehavior.Charge())
                {
                    animator.SetBool("Charge", true);
                }

                else if (enemyBehavior.AttackFound())
                {
                    animator.SetTrigger("Attack");
                }
            }
            else
            {
                enemyBehavior.Flip();

                if (enemyBehavior.Patrol())
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
