using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace RiverCrab
{
    public class HandManager : MonoBehaviour
    {
        Button cardbutton;
        RectTransform rect;
        [SerializeField, BoxGroup("儲存卡牌列表")]
        TextMeshProUGUI Hint;
        [HideInInspector]
        public List<Card> HandCard;
        [BoxGroup("儲存卡牌列表")]
        public List<GameObject> Hand = new List<GameObject>();
        KeyCode[] cardkey = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            KeyInput();
        }
        public bool HandCount()
        {
            if (Hand.Count >= 5)
            {
                Hint.text = "Hand Full !!";
                StartCoroutine(TextNull());
                return false;
            }
            return true;
        }        
        
        public void HandRemove(Card card,GameObject cardObj)
        {
            Hand.Remove(cardObj);
            HandCard.Remove(card);
        } 

        void KeyInput()
        {
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Input.GetKeyDown(cardkey[i]))
                {
                    print(i);
                    cardbutton = Hand[i].GetComponent<Button>();
                    cardbutton.onClick.Invoke();
                }
            }
        }
        //更新手牌位置
        public void PositionReset(GameObject gobj)
        {
            Hand.Remove(gobj);
            for (int i = 0; i < Hand.Count; i++)
            {
                rect = Hand[i].GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector3(-493f + (i * 150f), -10f, 0f);
            }
        }
        IEnumerator TextNull()
        {
            yield return new WaitForSeconds(0.2f);
            Hint.text = "";
        }
    }
}
