//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace RiverCrab
{
    //召喚投擲物卡牌的統一設置
    public class FlyObj : MonoBehaviour
    {
        [Header("獲取參考")]
        int destoryTime;
        Rigidbody2D rb;       
        [SerializeField]
        GenerateObj Gobj;
        [SerializeField]
        PlayerStatus pstatus;

        //投擲物的動量 旋轉 消耗
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();            
            pstatus = GameObject.Find("Player").GetComponent<PlayerStatus>();

            destoryTime = Gobj.ColliderTime;
            Vector2 ObjFly = (Gobj.tr.localRotation == Quaternion.Euler(0, 0, 0)) ? (new Vector2(Gobj.SpdX, Gobj.SpdY)) : (new Vector2(-Gobj.SpdX, Gobj.SpdY));
            float ObjSpin = (Gobj.tr.localRotation == Quaternion.Euler(0, 0, 0)) ? (-Gobj.SpdZ) : (Gobj.SpdZ);
            rb.velocity = ObjFly;
            rb.angularVelocity = ObjSpin;
            if (rb.velocity.x < 0f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            Destroy(gameObject,destoryTime);
        }

        //相反修正
        void Update()
        {

        }
        //打擊到物件所觸發的事件
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<EnemyNormal>().TakeDamage(Gobj.Damage, 0.1f);
                pstatus.EnergyAdd(Gobj.HitEnergy);
                //other.GetComponent<Enemy>().TakeDamage(GFO.Damage);
                if (!Gobj.AOE)
                {
                    Destroy(gameObject);
                }
            }
            else if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Wall"))
            {
                destoryTime--;
                if(destoryTime <= 0) Destroy(gameObject);
            }
        }        
    }
}
