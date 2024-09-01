using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RiverCrab
{
    public class CardUseButton : MonoBehaviour
    {
        [SerializeField]
        float cdnow;
        [SerializeField]
        float cd;
         
        [SerializeField]
        CardRare CR;
        [SerializeField]
        HandManager Hand;
        [SerializeField]
        Image RarityColor;       
        [SerializeField]
        Image Form;
        [SerializeField]
        CardEffect Eff;
        [SerializeField]
        int TimesOfUse;
        [SerializeField]
        string ObjTag;
        [SerializeField]
        bool UseOneTime;
        [SerializeField]
        Card card;

        [Header("獲取玩家參數")]
        [SerializeField]
        PlayerStatus Pstatus;       

        void OnDestroy()
        {
            print("destroy");
            Hand.PositionReset(gameObject);
            Hand.HandRemove(card,gameObject);
        }
        // Start is called before the first frame update
        void Start()
        {
            Pstatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
            CR = GameObject.Find("CardDraw").GetComponent<CardRare>();
            Hand = GameObject.Find("HandManager").GetComponent<HandManager>();

            RarityColor.color = CR.RarityColor;
            Form.sprite = CR.CardSprite;
            Eff = CR.cardeffect;
            TimesOfUse = CR.TimesOfUse;
            ObjTag = CR.ObjTag;
            card = CR.card;

            cd = CR.cardeffect.CDset;
            cdnow = cd;

            //cdnow = cardAxe.CDset;
            //cd = cardAxe.CDset;
        }

        // Update is called once per frame
        void Update()
        {
            //CD的畫面顯示
            RarityColor.fillAmount = cdnow / cd;
            Form.fillAmount = cdnow / cd;
            Form.color = ((cdnow / cd) >= 1) ? (new Color(255, 255, 255, 255)) : (Form.color);
        }

        public void Cooldown()
        {
            //卡牌冷卻
            if ((cdnow / cd) >= 1 && Pstatus.MP >= CR.cardeffect.MpCost)
            {
                TimesOfUse--;
                Pstatus.CostMp(CR.cardeffect.MpCost);
                GameObject target = GameObject.FindGameObjectWithTag(ObjTag);
                Eff.ApplyEffect(target);
                cdnow = 0;
                if (TimesOfUse<=0)
                {
                    Destroy(gameObject);
                    return;
                }
                StartCoroutine(CountDown());
            }
        }

        //CD回復
        IEnumerator CountDown()
        {
            while (cdnow < cd)
            {
                cdnow += Time.deltaTime;
                yield return null;
            }
        }
    }
}
