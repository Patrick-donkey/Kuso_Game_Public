using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverCrab
{
    public class AttackObj : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        float damage;
        [SerializeField]
        float lifeTime;
        [SerializeField]
        float flySpd;
        Rigidbody2D rb;
        [SerializeField]
        Transform tr;
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            tr = GameObject.Find("Player").transform;

            float Spd = (tr.localRotation == Quaternion.Euler(0, 0, 0)) ? flySpd : -flySpd;
            rb.AddForce(new Vector2(Spd, 0));

            Destroy(gameObject, lifeTime);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject);
                collision.gameObject.GetComponent<EnemyNormal>().TakeDamage(damage, 0);
            }
        }
    }   
}
