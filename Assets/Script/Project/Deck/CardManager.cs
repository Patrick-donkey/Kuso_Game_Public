using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RiverCrab
{
    public class CardManager : MonoBehaviour
    {
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
        

        [Header("卡牌列表")]
        [SerializeField]
        private List<Card> cards;
        public List<Card> Cards => cards;      
        //繼承用列表(跨場景)
        public static List<Card> FloorRewards = new List<Card>();       

        private void Awake()
        {
            cards.AddRange(FloorRewards);
            FloorRewards.Clear();
        }

        private void Start()
        {
            //Deck = CardImportButton.PickCards;
            //CardManager CM = new CardManager();            
        }        
    }

    [CustomEditor(typeof(CardManager))]
    public class CardAddUi : Editor
    {
        private int newCardTimeOfUse;
        private string newCardName;
        private string[] newCardObjTag = new string[] { "Player", "Enemy" };
        private string[] newCardType = new string[] { "Attack", "Defend", "Buff", "DeBuff" };
        private string[] newCardRarity = new string[] { "Common", "Rare", "Epic", "Legendary" };
        private Color newCardRarityColor = new Color(0, 0, 0, 255);
        private Sprite newCardSprite;
        private CardEffect newCardEffect;

        //設定可視化視窗參數
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CardManager CM = (CardManager)target;

            int TypeIndex = System.Array.IndexOf(newCardType, CM.Type);
            if (TypeIndex < 0) TypeIndex = 0;
            int ObjTagIndex = System.Array.IndexOf(newCardObjTag, CM.ObjTag);
            if (ObjTagIndex < 0) ObjTagIndex = 0;
            int RarityIndex = System.Array.IndexOf(newCardRarity, CM.Rarity);
            if (RarityIndex < 0) RarityIndex = 0;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Add New Card", EditorStyles.boldLabel);

            newCardTimeOfUse = EditorGUILayout.IntField("使用次數", newCardTimeOfUse);
            newCardName = EditorGUILayout.TextField("卡牌名稱", newCardName);
            ObjTagIndex = EditorGUILayout.Popup("使用對象", ObjTagIndex, newCardObjTag);
            TypeIndex = EditorGUILayout.Popup("卡牌類型", TypeIndex, newCardType);
            RarityIndex = EditorGUILayout.Popup("稀有度", RarityIndex, newCardRarity);
            newCardRarityColor = EditorGUILayout.ColorField("稀有度表示", newCardRarityColor);
            newCardSprite = (Sprite)EditorGUILayout.ObjectField("卡牌圖示", newCardSprite, typeof(Sprite), false);
            newCardEffect = (CardEffect)EditorGUILayout.ObjectField("卡牌效果", newCardEffect, typeof(CardEffect), false);
            //newUseOneTime = EditorGUILayout.Toggle("UseOneTime", newUseOneTime);

            CM.Type = newCardType[TypeIndex];
            CM.ObjTag = newCardObjTag[ObjTagIndex];
            CM.Rarity = newCardRarity[RarityIndex];
            //按下新增按鈕後初始化可視化視窗的參數
            if (GUILayout.Button("Add Card"))
            {
                Card newCard = new Card(newCardTimeOfUse, newCardName, CM.Type, CM.Rarity, newCardRarityColor, newCardSprite, newCardEffect, CM.ObjTag);
                CM.Cards.Add(newCard);

                // 清空輸入欄
                newCardTimeOfUse = 0;
                newCardName = "";
                ObjTagIndex = 0;
                TypeIndex = 0;
                RarityIndex = 0;
                newCardRarityColor = new Color(255, 255, 255, 255);
                newCardSprite = null;
                newCardEffect = null;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Card List", EditorStyles.boldLabel);
            //列出目前Cards內的所有卡牌參數
            foreach (var card in CM.Cards)
            {
                EditorGUILayout.LabelField("使用次數: " + card.TimesOfUse);
                EditorGUILayout.LabelField("卡牌名稱: " + card.Name);
                EditorGUILayout.LabelField("卡牌類型: " + card.Type);
                EditorGUILayout.LabelField("稀有度: " + card.Rarity);
                EditorGUILayout.LabelField("稀有度表示: " + card.RarityColor);
                EditorGUILayout.LabelField("使用對象: " + card.ObjTag);
                EditorGUILayout.ObjectField("卡牌圖示: ", card.Image, typeof(Sprite), false);
                EditorGUILayout.ObjectField("卡牌腳本: ", card.Effect, typeof(CardEffect), false);

                EditorGUILayout.Space();
            }
        }
    }
}
