using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace RiverCrab
{
    [CreateAssetMenu(menuName = "CardEffects/Attack/GeneralSpell")]
    public class GenerateSpell : CardEffect
    {
        Transform tr;
        HandManager handManager;
        [SerializeField]
        GameObject SpellObj;

        [Range(0, 100)]
        public float Damage;       
        public Vector3 foundsize;

        public override void ApplyEffect(GameObject target)
        {
            tr = target.transform;
            handManager = GameObject.Find("HandManager").GetComponent<HandManager>();
            Collider2D findtarget = Physics2D.OverlapBox(tr.position, foundsize, 0, LayerMask.GetMask("Enemy"));
            if (findtarget != null)
            {
                Instantiate(SpellObj, findtarget.transform.position+new Vector3(0,12,0), Quaternion.identity);                
            }
            else
            {
                Vector3 spawnPt = tr.localRotation == Quaternion.Euler(0,0,0)? new Vector3(20, 12, 0):new Vector3(-20,12, 0);
                Instantiate(SpellObj, tr.position + spawnPt, Quaternion.identity);                
            }
            Damage = Damage + Damage * handManager.Hand.Count;           
        }
    }
}
