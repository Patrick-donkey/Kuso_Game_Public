using UnityEngine;

namespace RiverCrab {
    public class Canine_Charge : StateMachineBehaviour
    {
        CanineBehaviour CB;
        EnemyCanine EC;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CB= animator.GetComponent<CanineBehaviour>();
            EC= animator.GetComponent<EnemyCanine>();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (EC.dead)
            {
                animator.SetTrigger("Death");
            }

            if (!CB.ChargePos())
            {
                animator.SetBool("Charge", false);
            }
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger("Death");
        }
    }

}