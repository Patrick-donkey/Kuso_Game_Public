using Sirenix.OdinInspector;
using UnityEngine;

//設定卡牌效果的ScriptableObj
namespace RiverCrab 
{
    [CreateAssetMenu(menuName = "CardEffects/Attack/GeneralObj")]
    public class GenerateObj : CardEffect
    {
        [Header("生成物與生成位置")]
        public Transform tr;
        public GameObject generateObj;
        
        [Range(0, 100), BoxGroup("物建屬性設置(子物件)")]
        public int ColliderTime;
        [Range(0, 100), BoxGroup("物建屬性設置(子物件)")]
        public float Damage;
        [Range(0, 100), BoxGroup("物建屬性設置(子物件)")]
        public float SpdX;
        [Range(0, 100), BoxGroup("物建屬性設置(子物件)")]
        public float SpdY;
        [Range(0, 1080), BoxGroup("物建屬性設置(子物件)")]
        public float SpdZ;
        [Range(0,10), BoxGroup("物建屬性設置(子物件)")]
        public float LifeTime;
        [Range(0, 100), BoxGroup("物建屬性設置(子物件)")]
        public float HitEnergy;
        [SerializeField, BoxGroup("物建屬性設置(子物件)")]
        float SpawnY;//預設飛行1 滾動-0.5
        [BoxGroup("物建屬性設置(子物件)")]
        public bool AOE;

        //方法執行

        //private void Awake() //訂閱事件
        //{
        //    OnApplyEffect += ApplyEffect;
        //}

        public override void ApplyEffect(GameObject target)
        {
            tr = target.transform;
            //Debug.Log($"Applying {damage} damage to {target.name}");
            Vector3 inspt = (tr.localRotation == Quaternion.Euler(0, 0, 0)) ? (new Vector3(2, SpawnY, 0)) : (new Vector3(-2, SpawnY, 0));
            Instantiate(generateObj, tr.position + inspt, Quaternion.identity);
        }        
    }
}
