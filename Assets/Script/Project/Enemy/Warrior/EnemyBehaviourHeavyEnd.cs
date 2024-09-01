using UnityEngine;

namespace RiverCrab {
    public class EnemyBehaviourHeavyEnd : StateMachineBehaviour
    {
        WarriorBehaviour EB;
        EnemyWarrior EW;
        
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            EB = animator.GetComponent<WarriorBehaviour>();
            EW = animator.GetComponent<EnemyWarrior>();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (EW.dead)
            {
                animator.SetTrigger("Death");
            }
            if (!EB.HeavyAttackFall())
            {
                animator.SetBool("HeavyAttack", false);
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger("Death");
        }
    } 
}
