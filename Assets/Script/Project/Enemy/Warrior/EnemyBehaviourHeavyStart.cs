//using RiverCrab;
//using UnityEngine;

//public class EnemyBehaviourHeavyStart : StateMachineBehaviour
//{
//    WarriorBehaviour EB;
//    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//        EB = animator.GetComponent<WarriorBehaviour>();
//    }

//    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//        if (EnemyChinken.isDead)
//        {
//            animator.SetTrigger("Death");
//        }
//        if (EB.HeavyAttackFall())
//        {
//            animator.SetBool("Fall",true);
//        }
//    }

//    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//        animator.SetBool("Jump",false);
//        animator.ResetTrigger("Death");
//    }
//}
