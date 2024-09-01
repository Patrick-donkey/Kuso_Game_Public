using UnityEngine;
namespace RiverCrab
{
    public class ChinkenEgg : StateMachineBehaviour
    {
        ChinkenBehaviour CB;
        EnemyChinken EC;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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

            if(EC.hit)
            {
                animator.SetTrigger("Hit");
            }

            if (!CB.EggShot()&&CB.AttackFound())
            {
                animator.SetTrigger("Attack");
            }
            if (!CB.PlayerFound())
            {
                animator.SetBool("Egg", false);
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger("Attack");
        }
    }
}
