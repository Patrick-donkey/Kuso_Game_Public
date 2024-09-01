using UnityEngine;

namespace RiverCrab
{
    //召喚投擲物卡牌的統一設置
    public class RollObj : MonoBehaviour
    {
        [Header("獲取參考")]
        [SerializeField]
        Rigidbody2D rb;
        [SerializeField]
        GenerateObj Gobj;
        [SerializeField]
        PlayerStatus pstatus;
        PolygonCollider2D p2d; 
        float time = 0;

        //滾動物件的動量及生命週期
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            p2d = GetComponent<PolygonCollider2D>();
            pstatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
            
            time = Gobj.LifeTime;            
            //rb.velocity = new Vector2(GRO.RollSpdx, rb.velocity.y);
            Destroy(gameObject, time);
        }
        private void Update()
        {
            time -= Time.deltaTime;
            transform.localRotation = (Gobj.tr.position.x - transform.position.x < -0.1f) ? Quaternion.Euler(0, 0, 45) : Quaternion.Euler(0, 180, 45);
            rb.velocity += (Gobj.tr.position.x-transform.position.x<-0.1f) ? new Vector2(Gobj.SpdX, 0) : new Vector2(-Gobj.SpdX, 0);            
        }

        //觸碰到牆壁物件損壞
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.transform.parent = gameObject.transform;
                collision.gameObject.GetComponent<EnemyNormal>().TakeDamage(0, 0);
                if (time<0.1f)
                {
                    print("Stop");
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (0,0);
                    collision.gameObject.GetComponent<EnemyNormal>().TakeDamage(Gobj.Damage, 0);
                }
            }
            if (collision.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }        
    }
}
