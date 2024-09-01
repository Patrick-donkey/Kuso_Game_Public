//using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace RiverCrab
{
    public class PlayerKuso : MonoBehaviour
    {

        [SerializeField, BoxGroup("玩家移動參數")] float brustSpd;
        [SerializeField, BoxGroup("玩家移動參數")] float hSpd;
        [SerializeField, BoxGroup("玩家移動參數")] float jump = 8f;

        //狀態檢測
        bool isground;
        bool cooldown = true;

        [SerializeField, BoxGroup("玩家攻擊組件")]
        GameObject ATKobj;

        [Header("獲取玩家組件")]
        Rigidbody2D rb;        
        Animator anim;
        CapsuleCollider2D feet;
        PlayerStatus status;        

        void Start()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            feet = GetComponent<CapsuleCollider2D>();
            status = GetComponent<PlayerStatus>();             
        }

        // Update is called once per frame
        void Update()
        {
            if (GM.freeze)
            {
                rb.velocity = Vector2.zero;
                return;
            }
            Move();
            Flip();
            Ground();
            SwitchMovtation();
            Jump();
            if (!GM.isbattle) return;
            ATK();
        }


        void Move()
        {
            float hspd = (Input.GetKey(KeyCode.LeftShift)) ? (Input.GetAxis("Horizontal") * brustSpd) : (Input.GetAxis("Horizontal") * hSpd);
            Vector2 move = new Vector2(hspd, rb.velocity.y);
            if(!status.hit)rb.velocity = move;
            anim.SetFloat("hSpd", Mathf.Abs(move.x));            
        }

        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space)&&isground)
            {
                anim.SetBool("Jump",true);
                Vector2 jp = new Vector2(rb.velocity.x, jump);
                rb.velocity = jp;
            }
            anim.SetFloat("vSpd", rb.velocity.y);
        }

        void Flip()
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1f) return;
            transform.eulerAngles = rb.velocity.x > 0 ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);            
        }

        //地面檢測
        void Ground()
        {
            isground = feet.IsTouchingLayers(LayerMask.GetMask("Ground")) || feet.IsTouchingLayers(LayerMask.GetMask("Enemy"));
            if (!isground)
            {
                anim.SetFloat("vSpd", rb.velocity.y);
            }            
        }

        

        void SwitchMovtation()
        {
            if (anim.GetBool("Jump") && rb.velocity.y < -0.1f)
            {
                anim.SetBool("Jump", false);
            }
        }

        void ATK()
        {
            if (Input.GetKey(KeyCode.Mouse1) && cooldown)
            {
                anim.SetTrigger("Attack");                
                StartCoroutine(ATKCD());
            }
        }

        IEnumerator ATKCD()
        {
            cooldown = false;               
            yield return new WaitForSeconds(1f);
            anim.ResetTrigger("Attack");
            cooldown = true;
        }

        private void GenerateATKobj()
        {
            Vector3 inspt = (transform.localRotation == Quaternion.Euler(0, 0, 0)) ? (new Vector3(1, 0, 0)) : (new Vector3(-1, 0, 0));
            Instantiate(ATKobj, transform.position + inspt, Quaternion.identity);
        }
    }
}

