using RiverCrab;
using UnityEngine;

public class weapon : MonoBehaviour
{
    private float damage = 1;
    private float hitforce = 0;
    Rigidbody2D rig;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {

        rig.AddForce(Vector2.left);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyNormal>().TakeDamage(damage, hitforce);
        }
    }

}
