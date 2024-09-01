using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

//抽卡的各項參數
namespace RiverCrab
{
    [System.Serializable]
    public class CardRare : MonoBehaviour
    {
        [BoxGroup("稀有度變色參數")]        
        public Color[] colors;
        [SerializeField,BoxGroup("稀有度變色參數")]
        float fadeDuration = 1.0f;
        [SerializeField, BoxGroup("稀有度變色參數")]
        Image RareColorChange;

        [BoxGroup("抽卡能量")]
        public int DrawEnegry;
        [SerializeField, BoxGroup("抽卡能量")]
        int DrawEnegrynow;
        [SerializeField, BoxGroup("抽卡能量")]
        Image DrawEnergyForm;

        Animator anim;        
        Canvas canvas;
        GameObject cardUi;
        HandManager Hand;

        [HideInInspector]
        public int TimesOfUse;
        [HideInInspector]
        public string Name;
        [HideInInspector]
        public string Type;
        [HideInInspector]
        public string ObjTag;
        [HideInInspector]
        public string Rarity;
        [HideInInspector]
        public Color RarityColor;
        [HideInInspector]
        public Sprite CardSprite;
        [HideInInspector]
        public CardEffect cardeffect;
        [HideInInspector]
        public Card card;

        [SerializeField, BoxGroup("儲存卡牌列表")]
        GameObject CardUseButton;
        [SerializeField, BoxGroup("儲存卡牌列表")]
        List<Card> DrawDeck;
        string[] rarerank = new string[] { "Common", "Rare", "Epic", "Legendary", "" };
        #region 稀有分級管理卡牌
        //[SerializeField]
        //List<Card> cards = new List<Card>();
        //[SerializeField] 稀有分層管理
        //List<Card>[] RarityCheck = new List<Card>[3];
        //public List<Card> Cards => cards;
        #endregion

        #region 事件
        void Start()
        {
            anim = GetComponent<Animator>();
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            Hand = GameObject.Find("HandManager").GetComponent<HandManager>();
            cardUi = GameObject.Find("CardUI");

            DrawDeck = new List<Card>();
            //for(int i = 0;i < rarerank.Length; i++) { DrawDeck = GetCardsByRarity(rarerank[i]); }
        }

        private void Update()
        {
#if UNITY_EDITOR
            DrawEnegry = 100; //測試用能量
#endif
            if (DrawEnegry > 100) DrawEnegry = 100;
            DrawEnergyForm.fillAmount = (float)DrawEnegry / 100;
            ColorChange();            
            //CheckDeck();
        }
        #endregion

        #region 稀有度顏色
        //稀有度變色檢測
        void ColorChange()
        {
            if (DrawEnegry != DrawEnegrynow)
            {
                DrawEnegrynow = DrawEnegry;
                int changecount = 0;
                DrawDeck.Clear();
                for (int i = DrawEnegry; i >= 25; i -= 25)
                {
                    changecount++;
                    //分級洗牌
                    //if (RarityCheck[changecount-1].Count <= 0)
                    //{
                    //    RarityCheck[changecount-1] = GetCardsByRarity(rarerank[changecount - 1]);                   
                    //}                
                    //DrawDeck.RemoveAll(card => card.Rarity == rarerank[changecount]);
                    if (DrawDeck.Any(card => card.Rarity == rarerank[changecount - 1]) == false)
                    {
                        var cards = GetCardsByRarity(rarerank[changecount - 1]);
                        DrawDeck.AddRange(cards);
                    }
                    StartCoroutine(ChangeColorWithFade(changecount));
                }
            }
        }

        //稀有度顏色轉變
        IEnumerator ChangeColorWithFade(int index)
        {
            Color startColor = RareColorChange.color;
            Color endColor = colors[index];

            float elapsedTime = 0.0f;
            while (elapsedTime < fadeDuration)
            {
                float t = elapsedTime / fadeDuration;
                RareColorChange.color = Color.Lerp(startColor, endColor, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            RareColorChange.color = endColor;
            yield return null;
        }
        #endregion

        #region 抽卡
        //抽牌檢驗
        public void Draw()
        {
            //手牌到達上限返回            

            if (DrawEnegry >= 25&&Hand.HandCount())
            {
                DrawEnegry = 0;
                anim.SetBool("CardFly", true);                
                //StartCoroutine(Drawanim());
                Card draw = DrawCard();
                if (draw != null)
                {
                    Debug.Log(draw.Name + draw.Type + draw.Rarity);
                    GameObject Preview = Instantiate(CardUseButton, canvas.transform);
                    Hand.Hand.Add(Preview);
                    RectTransform rect = Preview.GetComponent<RectTransform>();
                    rect.SetParent(cardUi.transform,true);
                    rect.anchoredPosition = new Vector3(-493f + (Hand.Hand.IndexOf(Preview) * 150f), -10f, 0f);

                    StartCoroutine(AnimBack());                    
                }
                else
                {
                    Debug.Log("Deck null");
                }
            }
        }
        // 抽牌
        Card DrawCard()
        {
            if (DrawDeck.Count > 0)
            {
                Shuffle();
                Card drawnCard = DrawDeck[0];  //RarityCheck[checkindex][0] 分級管理;
                TimesOfUse = drawnCard.TimesOfUse;
                Name = drawnCard.Name;
                Type = drawnCard.Type;
                Rarity = drawnCard.Rarity;
                RarityColor = drawnCard.RarityColor;
                CardSprite = drawnCard.Image;
                cardeffect = drawnCard.Effect;
                ObjTag = drawnCard.ObjTag;
                card = drawnCard;                

                Hand.HandCard.Add(drawnCard);

                return drawnCard;
            }
            else
            {
                return null; // 或者可以返回一個特殊的“沒有牌”的卡牌
            }
        }

        //獲取列表內的對應稀有度卡牌
        public static List<Card> GetCardsByRarity(string rarity)
        {
            return CardImportButton.PickCards.Where(card => card.Rarity == rarity).ToList();
        }

        //抽牌結束後重製抽卡物件
        IEnumerator AnimBack()
        {
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("CardFly", false);
            StartCoroutine(ChangeColorWithFade(0));
        }
        #endregion

        // 洗牌
        public void Shuffle()
        {
            // 使用 Fisher-Yates 洗牌算法
            System.Random rng = new System.Random();
            int n = DrawDeck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = DrawDeck[k];
                DrawDeck[k] = DrawDeck[n];
                DrawDeck[n] = value;
            }
        }

        //#region 手牌管理
        //void KeyInput()
        //{
        //    for(int i = 0; i < Hand.Count; i++)
        //    {                
        //        if (Input.GetKeyDown(cardkey[i]))
        //        {
        //            print(i);
        //            cardbutton = Hand[i].GetComponent<Button>();
        //            cardbutton.onClick.Invoke();
        //        }
        //    }
        //}
        ////更新手牌位置
        //public void PositionReset(GameObject gobj)
        //{
        //    Hand.Remove(gobj);
        //    for (int i=0; i < Hand.Count; i++)
        //    {
        //        rect = Hand[i].GetComponent<RectTransform>();
        //        rect.anchoredPosition = new Vector3(-493f + (i * 150f), -250f, 0f);
        //    }  
        //}        
        //IEnumerator TextNull()
        //{
        //    yield return new WaitForSeconds(0.2f);
        //    Hint.text = "";
        //}
        //#endregion

        //生成抽牌動畫物件
        //IEnumerator Drawanim()
        //{
        //    yield return new WaitForSeconds(0.5f);

        //    GameObject interview = Instantiate(CarInterview, canvas.transform);
        //    RectTransform rect = interview.GetComponent<RectTransform>();
        //    CardPreview view = interview.GetComponent<CardPreview>();

        //    rect.anchoredPosition = new Vector2(1000.0f, 0f);
        //}
    }
}

