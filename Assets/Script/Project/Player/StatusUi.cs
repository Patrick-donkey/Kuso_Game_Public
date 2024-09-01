//using System.Collections;
//using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RiverCrab
{
    public class StatusUi : MonoBehaviour
    {
        [Header("狀態欄組件")]
        [SerializeField]
        TextMeshProUGUI[] txt;
        [SerializeField]
        Image[] img;

        [Header("玩家狀態欄參數")]
        public static float Hpcurrrent;
        public static float Hpmax;
        public static float Mpcurrrent;
        public static float Mpmax;
        public static float Egcurrrent = 0;
        public static float Egmax;


        //狀態欄百分比設置
        void Update()
        {
            img[0].fillAmount = Hpcurrrent / Hpmax;
            img[1].fillAmount = Mpcurrrent / Mpmax;
            img[2].fillAmount = Egcurrrent / Egmax;

            txt[0].text = Hpcurrrent.ToString() + " / " + Hpmax.ToString();
            txt[1].text = Mpcurrrent.ToString() + " / " + Mpmax.ToString();
            txt[2].text = Egcurrrent.ToString() + " / " + Egmax.ToString();
        }
    }
}
