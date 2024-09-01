using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverCrab
{
    public class DrawEnergyCrystal : MonoBehaviour
    {
        CardRare CR;
        Rigidbody2D Rb;
        [SerializeField]
        int EnergyAdd;
        [SerializeField]
        float DestroyTime;
        // Start is called before the first frame update    

        private void Start()
        {
            CR = GameObject.Find("CardDraw").GetComponent<CardRare>();
            Rb = GetComponent<Rigidbody2D>();

            Destroy(gameObject, DestroyTime);
        }

        // Update is called once per frame
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                CR.DrawEnegry += EnergyAdd;
                Destroy(gameObject);
            }
            if (collision.gameObject.CompareTag("Ground"))
            {
                Rb.gravityScale = 0f;
                Rb.velocity = new Vector2(Rb.velocity.x, 0);
            }
        }
    }
}
