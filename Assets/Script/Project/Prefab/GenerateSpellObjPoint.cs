using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverCrab {
    public class GenerateSpellObjPoint : MonoBehaviour
    {
        float damage;        
        [SerializeField]
        GameObject[] spawnObj;
        [SerializeField]
        Transform AreaUP;
        [SerializeField]
        Transform AreaDown;
        [SerializeField]
        GenerateSpell GS;
        BoxCollider2D b2d;

        private void Start()
        {
            b2d = GetComponent<BoxCollider2D>();
            damage = GS.Damage;
            for (int i = 0; i < 10; i++)
            {
                int spawnIndex = Random.Range(0, spawnObj.Length);
                Vector2 randompos = RandomPos();
                GameObject obj = Instantiate(spawnObj[spawnIndex], randompos, Quaternion.identity);
                obj.transform.parent = transform;
            }
            Destroy(gameObject, 5f);
        }
        Vector2 RandomPos()
        {
            Vector2 randomPos = new Vector2(Random.Range(AreaDown.position.x, AreaUP.position.x), Random.Range(AreaDown.position.y, AreaUP.position.y));
            return randomPos;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                b2d.enabled = false;                
                StartCoroutine(DamageEnemy(collision));
            }
        }
        IEnumerator DamageEnemy(Collider2D collision)
        {            
            print("damage");
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage, 0);
            yield return new WaitForSeconds(0.5f); 
            b2d.enabled=true;
        }
    } 
}
