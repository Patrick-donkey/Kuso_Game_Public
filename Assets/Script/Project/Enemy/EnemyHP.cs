using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//怪物狀態欄
namespace RiverCrab
{
    public class EnemyHP : MonoBehaviour
    {
        public static float Ehcurrrent;
        public static float Ehmax;
        [SerializeField]
        float HealthPosY;
        [SerializeField]
        Image Ehealthline;
        [SerializeField]
        Transform ObjTr;        

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Ehealthline.fillAmount = Ehcurrrent / Ehmax;
            if (ObjTr == null)
            {
                Destroy(gameObject);
                return;
            }
            transform.position = ObjTr.position + new Vector3(0, HealthPosY, 0);
        }
    }
}
