using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace RiverCrab
{
    public class EnemyNormal : Enemy
    {
        [BoxGroup("能量物件掉落")]
        int TakeDamageCount = 0;
        [SerializeField, BoxGroup("能量物件掉落")]
        int TakeDamageCountMax;
        [SerializeField, BoxGroup("能量物件掉落")]
        int EnergyAdd;
        bool dropped;        
        CardRare CR;

        [SerializeField, BoxGroup("受擊震動參數")]        
        float vibrationSpeed;
        [SerializeField, BoxGroup("受擊震動參數")]
        float vibrationStrength;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            CR = GameObject.Find("CardDraw").GetComponent<CardRare>();            
        }
        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        protected override void Dead()
        {
            base.Dead();            
           
            if (dropped) return;
            dropped = true;

            int DrawEnergy = Random.Range(1, 4);
            for (int i = 0; i < DrawEnergy; i++)
            {
                Instantiate(DrawEnergyObj, transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 0, 0), Quaternion.identity);
            }            
            //Destroy(transform.parent.gameObject,deadTime);
        }            

        public override void TakeDamage(float damage, float hitforce)
        {
            base.TakeDamage(damage, hitforce);

            if (HP > 0 && !hit)
            {
                StartCoroutine(Shake());
                StartCoroutine(EHPerform());
            }            
            //StartCoroutine(StopAddForce());

            TakeDamageCount++;
            if (TakeDamageCount >= TakeDamageCountMax)
            {
                TakeDamageCount = 0;
                int DrawEnergy = Random.Range(1, 3);
                //直接增加抽卡能量
                CR.DrawEnegry += (DrawEnergy * EnergyAdd);

                //產生掉落物玩家撿取後增加抽卡能量
                //for (int i = 0; i < DrawEnergy; i++)
                //{
                //    Instantiate(DrawEnergyObj, transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 0, 0), Quaternion.identity);
                //}
            }
        }
        
        IEnumerator EHPerform()
        {
            EHPLine.SetActive(true);
            yield return new WaitForSeconds(5.0f);            
            EHPLine.SetActive(false);
        }

        IEnumerator Shake()
        {
            hit = true;
            for (int i = 0; i < 10; i++)
            {
                transform.localPosition = transform.localPosition + Random.insideUnitSphere * 0.1f;
                yield return new WaitForSeconds(0.02f);
            }
            yield return new WaitForSeconds(0.2f);
            transform.localPosition = transform.localPosition;
            hit = false;
        }
    }
}
