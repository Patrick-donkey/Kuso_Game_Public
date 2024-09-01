using RiverCrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellObj : MonoBehaviour
{
    [SerializeField]
    float spdY;
    [SerializeField]
    float spdZ;

    Rigidbody2D rb;    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();       
        rb.velocity = new Vector2(rb.velocity.x, spdY);
        rb.angularVelocity = spdZ;
    }    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SpellArea"))
        {
            rb.velocity = new Vector2(rb.velocity.x, -spdY);            
        }
    }
    private void Update()
    {
        if (rb.velocity.y > 0f)
        {
            rb.gravityScale = -2;
        }
        else rb.gravityScale = 2;
    }
}
