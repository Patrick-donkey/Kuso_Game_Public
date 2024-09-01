//using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace RiverCrab
{
    public class BoxATK : MonoBehaviour
    {
        [SerializeField, Range(0, 100)]
        float Damage;
        [SerializeField, Range(0, 10)]
        float ATKCD;
        [SerializeField]
        BoxCollider2D b2d;

        //攻擊檢測
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
            {
                print("Hit");
                b2d.enabled = false;
                other.gameObject.GetComponent<PlayerStatus>().TakeDamage(Damage);
            }
        }
    }
}
