using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RiverCrab
{
    public class ChestReward : MonoBehaviour
    {
        [Header("寶箱參數")]
        [SerializeField]
        bool HasOpen;
        [SerializeField]
        bool PlayerNearby;
        [SerializeField]
        Animator anim;        

        [Header("獎勵列表與物件")]
        [SerializeField]
        int RewardsCount;
        [SerializeField]
        List<Card> Rewards;
        public List<Card> RewardsList => Rewards;
        [SerializeField]
        GameObject CardPreview;
        [SerializeField]
        Canvas canvas;

        // Start is called before the first frame update
        private void Start()
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerNearby = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerNearby = false;
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (PlayerNearby && !HasOpen && Input.GetKeyDown(KeyCode.E))
            {
                HasOpen = true;
                anim.SetTrigger("Open");
                
                for (int i = 0; i < RewardsCount; i++)
                {
                    CardManager.FloorRewards.Add(Rewards[i]);

                    GameObject preview = Instantiate(CardPreview, canvas.transform);
                    RectTransform rect = preview.GetComponent<RectTransform>();
                    RewardsPreview view = preview.GetComponent<RewardsPreview>();
                    Card rewards = Rewards[i];

                    view.Form.sprite = rewards.Image;
                    view.Name.text = rewards.Name;
                    view.RarityColor.color = rewards.RarityColor;
                    view.Type.text = rewards.Type;

                    rect.anchoredPosition = new Vector2(1000.0f, 0f);                    
                }
            }
        }
    }
    [CustomEditor(typeof(ChestReward))]
    public class RewardAddUi : Editor
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

            ChestReward CR = (ChestReward)target;

            int TypeIndex = System.Array.IndexOf(newCardType, CR.RewardsList[0].Type);
            if (TypeIndex < 0) TypeIndex = 0;
            int ObjTagIndex = System.Array.IndexOf(newCardObjTag, CR.RewardsList[0].ObjTag);
            if (ObjTagIndex < 0) ObjTagIndex = 0;
            int RarityIndex = System.Array.IndexOf(newCardRarity, CR.RewardsList[0].Rarity);
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

            CR.RewardsList[0].Type = newCardType[TypeIndex];
            CR.RewardsList[0].ObjTag = newCardObjTag[ObjTagIndex];
            CR.RewardsList[0].Rarity = newCardRarity[RarityIndex];
            //按下新增按鈕後初始化可視化視窗的參數
            if (GUILayout.Button("Add Card"))
            {
                Card newCard = new Card(newCardTimeOfUse, newCardName, CR.RewardsList[0].Type, CR.RewardsList[0].Rarity, newCardRarityColor, newCardSprite, newCardEffect, CR.RewardsList[0].ObjTag);
                CR.RewardsList.Add(newCard);

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
            foreach (var card in CR.RewardsList)
            {
                EditorGUILayout.LabelField("使用次數: " + card.TimesOfUse);
                EditorGUILayout.LabelField("卡牌名稱: " + card.Name);
                EditorGUILayout.LabelField("卡牌類型: " + card.Type);
                EditorGUILayout.LabelField("稀有度: " + card.Rarity);
                EditorGUILayout.LabelField("稀有度表示: " + card.RarityColor);
                EditorGUILayout.LabelField("使用對象: " + card.ObjTag);
                EditorGUILayout.ObjectField("卡牌圖示: ", card.Image, typeof(Sprite), false);
                EditorGUILayout.ObjectField("卡牌效果: ", card.Effect, typeof(CardEffect), false);

                EditorGUILayout.Space();
            }
        }
    }
}
