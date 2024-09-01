//using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RiverCrab
{
    public class Enemy : MonoBehaviour
    {
        [BoxGroup("怪物狀態")]
        public float HP;
        [HideInInspector,BoxGroup("怪物狀態")]
        public bool dead;
        [HideInInspector,BoxGroup("怪物狀態")]
        public bool hit;
        float HPmax;

        [SerializeField,BoxGroup("參考物件")]
        protected GameObject EHPLine;
        [SerializeField,BoxGroup("參考物件")]
        protected GameObject DrawEnergyObj;
        [SerializeField,BoxGroup("參考物件")]
        protected GameObject DamageFloat;
        Color orginc;       
        SpriteRenderer sr;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            HPmax = HP;
            orginc = sr.color;          
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            
        }

        //死亡後事件
        protected virtual void Dead()
        {
            HP = 0;
            dead = true;
            //Destroy(gameObject.transform.parent.gameObject, deadTime);
        }

        //受傷事件(傷害值,擊退力度)
        public virtual void TakeDamage(float damage, float hitforce)
        {
            //受傷
            HP -= damage;
            EnemyHP.Ehmax =HPmax;
            EnemyHP.Ehcurrrent = HP;

            if (HP <= 0)
            {
                Dead();
            }

            //閃爍
            Flash(0.2f);

            //傷害值顯示
            if (damage > 1f)
            {
                GameObject gb = Instantiate(DamageFloat, transform.position, Quaternion.identity);
                gb.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();
            }
        }

        //受擊閃爍
        void Flash(float time)
        {
            sr.color = Color.red;
            Invoke("Refresh", time);
        }

        //閃爍復原
        void Refresh()
        {
            sr.color = orginc;
        }
    }
}
