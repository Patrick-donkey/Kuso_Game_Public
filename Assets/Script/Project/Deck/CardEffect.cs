//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

//物件屬性設置(父物件)
namespace RiverCrab
{
    public abstract class CardEffect : ScriptableObject
    {
        [Header("卡牌公用屬性")]
        [SerializeField, Range(0, 100)]
        public float MpCost;
        [SerializeField, Range(0, 20)]
        public float CDset;
        [SerializeField, Range(0, 20)]
        public float EnegryAdd;

        //委託方法
        
        public abstract void ApplyEffect(GameObject target);

        //public delegate void EffectAction(GameObject target);
        //public event EffectAction OnApplyEffect;
        //void TriggerEffect(GameObject target)
        //{
        //    OnApplyEffect?.Invoke(target);            
        //}
    }
}
