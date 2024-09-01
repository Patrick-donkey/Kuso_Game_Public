using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RiverCrab
{
    //將卡牌導入到選牌介面
    public class CardImportButton : MonoBehaviour
    {
        int index = 1;
        [SerializeField]
        CardManager CM;

        [SerializeField, Header("滿牌提示")]
        TextMeshProUGUI hint;
        [SerializeField, Header("牌庫按鈕獲取")]
        public GameObject[] Buttons;
        [SerializeField]
        GameObject PickButton;
        [SerializeField]
        Transform PickArea;

        [Header("牌庫與牌組")]
        [SerializeField]
        public List<Card> CardItems = new List<Card>();
        public static List<Card> PickCards = new List<Card>();
        //[SerializeField]
        //List<Card> Checkcard =new List<Card>();
        // Start is called before the first frame update

        void Start()
        {
            CM = GameObject.Find("CM").GetComponent<CardManager>();

            CardItems = CM.Cards;

            for(int i = 0; i < CardItems.Count; i++)
            {
                GameObject button = Instantiate(PickButton, PickArea);
                string Name = "CardItem";
                int Num = i;
                button.name = $"{Name} ({i})";
            }

            Buttons = new GameObject[CardItems.Count];
            for (int i = 0; i < CardItems.Count; i++)
            {
                int index = i;
                Buttons[index] = GameObject.Find("CardItem (" + index + ")");
                Button button = Buttons[index].GetComponent<Button>();
                button.onClick.AddListener(() => PickUpCard(index));
                Import(index);
            }
        }
        //獲取卡牌屬性並套用至對應按鈕
        private void Import(int index)
        {
            Transform rare = Buttons[index].transform.GetChild(0);
            Transform form = Buttons[index].transform.GetChild(1);
            Transform name = Buttons[index].transform.GetChild(2);
            Transform type = Buttons[index].transform.GetChild(3);

            Image formimage = form.GetComponent<Image>();
            TextMeshProUGUI nametxt = name.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI typetxt = type.GetComponent<TextMeshProUGUI>();
            Image rarecolor = rare.GetComponent<Image>();

            formimage.sprite = CardItems[index].Image;
            nametxt.text = CardItems[index].Name;
            typetxt.text = CardItems[index].Type;
            rarecolor.color = CardItems[index].RarityColor;
        }
        //選牌與取消
        public void PickUpCard(int slotIndex)
        {
            if (!PickCards.Contains(CardItems[slotIndex]))
            {
                if (PickCards.Count > 5)
                {
                    hint.text = "Can't Add More Card !!";
                    StartCoroutine(hintnull());
                    return;
                }
                PickCards.Add(CardItems[slotIndex]);
                Buttons[slotIndex].GetComponent<Image>().color = new Color(0, 0.75f, 1, 1);
                Buttons[slotIndex].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = $"{index}";
                index++;
            }
            else
            {
                PickCards.Remove(CardItems[slotIndex]);
                Buttons[slotIndex].GetComponent<Image>().color = new Color(0, 0.75f, 1, 0);
                Buttons[slotIndex].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "";
                index--;
                UpdateCardIndices();
            }
        }
        //排列選牌順序
        private void UpdateCardIndices()
        {
            int updatedIndex = 1; // 從 1 開始重新計算索引
            foreach (var card in PickCards)
            {
                int cardSlotIndex = CardItems.IndexOf(card); // 獲取卡片在 CardItems 列表中的索引
                Buttons[cardSlotIndex].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = $"{updatedIndex}";
                updatedIndex++;
            }
        }
        IEnumerator hintnull()
        {
            yield return new WaitForSeconds(0.2f);
            hint.text = "";
        }        
    }
}
