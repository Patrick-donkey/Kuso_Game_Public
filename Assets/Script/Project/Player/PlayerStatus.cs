//using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

namespace RiverCrab
{
    public class PlayerStatus : MonoBehaviour
    {
        [SerializeField, Range(0, 100),BoxGroup("玩家參數與狀態")]
        public float HP;
        [SerializeField, Range(0, 10), BoxGroup("玩家參數與狀態")]
        float heal;
        [SerializeField, Range(0, 100), BoxGroup("玩家參數與狀態")]
        public float MP;
        [SerializeField, Range(0, 100), BoxGroup("玩家參數與狀態")]
        public float Energy;
        [SerializeField, Range(0, 10), BoxGroup("玩家參數與狀態")]
        float mpRecover;
        [SerializeField, Range(0, 100), BoxGroup("玩家參數與狀態")]
        float hitCD;
        public bool hit;
        bool healing;
        bool isDead;
        bool Withered;
        bool isrecover;
        bool takebreak;
        public static bool Charged;

        [Header("玩家組件")]
        [SerializeField]
        Rigidbody2D rb;
        [SerializeField]
        BoxCollider2D b2d;
        [SerializeField]
        Animator anim;
        [SerializeField]
        Renderer myrender;
        [SerializeField]
        GameObject Fail;

        // Start is called before the first frame update    

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            b2d = GetComponent<BoxCollider2D>();
            anim = GetComponent<Animator>();
            myrender = GetComponent<Renderer>();

            StatusUi.Hpmax = HP;
            StatusUi.Mpmax = MP;
            StatusUi.Egmax = Energy;
            StatusUi.Hpcurrrent = HP;
            StatusUi.Mpcurrrent = MP;
            Energy = 0;

        }

        //防止超過最大值
        void Update()
        {
#if UNITY_EDITOR
            //HP = 100;
            MP = 20;
#endif
            if (HP > StatusUi.Hpmax)
            {
                healing = false;
                HP = StatusUi.Hpmax;
            }
            if (MP > StatusUi.Mpmax) MP = StatusUi.Mpmax;
            if (Energy > StatusUi.Egmax) Energy = StatusUi.Egmax;
        }

        //受傷管理
        public void TakeDamage(float damage)
        {
            HP -= damage;
            StatusUi.Hpcurrrent = HP;
            StartCoroutine(Doblink(2,0.2f));
            if(!healing)StartCoroutine(Heal());
            if (HP <= 0)
            {
                HP = 0;
                rb.velocity = Vector3.zero;
                b2d.enabled = false;
                anim.SetTrigger("Death");
            }
            //c2d.enabled = false;            
            //StartCoroutine(HitCD());
        }

        public void Dead()
        {
            Fail.SetActive(true);
            Destroy(gameObject);
        }

        IEnumerator Heal()
        {
            healing = true;
            while (StatusUi.Hpcurrrent < StatusUi.Hpmax)
            {
                HP += heal;
                StatusUi.Hpcurrrent = HP;
                yield return new WaitForSeconds(1f);
            }
        }

        //受擊特效
        IEnumerator Doblink(int numblink,float sec)
        {
            anim.SetTrigger("Hit");
            for (int i = 0; i < numblink * 2; i++)
            {
                myrender.enabled = !myrender.enabled;
                yield return new WaitForSeconds(sec);
            }
            anim.ResetTrigger("Hit");
            myrender.enabled = true;
        }

        //重製受傷檢驗區
        IEnumerator HitCD(Transform enemy)
        {
            hit = true;            
            anim.SetTrigger("Hit");
            yield return new WaitForSeconds(hitCD);
            anim.ResetTrigger("Hit");
            hit = false;
            //c2d.enabled = true;
        }

        //耗魔管理
        public void CostMp(float cost)
        {
            MP -= cost;
            StatusUi.Mpcurrrent = MP;
            if (MP < 0)
            {
                MP = 0;
                Withered = true;
                StartCoroutine(Wither());
            }
            if (isrecover) return;
            StartCoroutine(MpRecover());
        }

        //魔力乾枯
        IEnumerator Wither()
        {
            yield return new WaitForSeconds(3.0f);
            Withered = false;
        }

        //魔力回復
        IEnumerator MpRecover()
        {
            isrecover = true;
            while (!Withered && MP < StatusUi.Mpmax)
            {
                MP += mpRecover;
                yield return new WaitForSeconds(0.5f);
                StatusUi.Mpcurrrent = MP;
            }
            yield return null;
            isrecover = false;
        }

        //能量管理
        public void EnergyAdd(float energy)
        {
            if (takebreak) return;
            Energy += energy;
            StatusUi.Egcurrrent = Energy;
            if (Energy >= StatusUi.Egmax)
            {
                Charged = true;
            }
        }

        //變身結束疲憊
        public void EnergyClear()
        {
            Energy = 0;
            StatusUi.Egcurrrent = Energy;
            StartCoroutine(Tired());
        }

        //疲勞
        IEnumerator Tired()
        {
            takebreak = true;
            yield return new WaitForSeconds(10.0f);
            takebreak = false;
        }
        public void KnockBack(Vector2 force,Transform enemyPos)
        {
            StartCoroutine(HitCD(enemyPos));
            Vector2 addForce = enemyPos.position.x - transform.position.x < 0.0f ? force : -force;
            rb.AddForce(addForce);
        }
    }
}
