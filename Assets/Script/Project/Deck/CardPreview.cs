//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;


//namespace RiverCrab
//{
//    //抽取到的卡牌動畫
//    public class CardPreview : MonoBehaviour
//    {
//        [Header("生成卡牌按鈕")]
//        [SerializeField]
//        GameObject Card;

//        [Header("獲取參考")]
//        [SerializeField]
//        Canvas canvas;
//        Animator anim;
//        [SerializeField]
//        CardRare CR;

//        [Header("獲取卡牌參數")]
//        [SerializeField]
//        Image Form;
//        [SerializeField]
//        Image RarityColor;
//        [SerializeField]
//        TextMeshProUGUI Name;
//        [SerializeField]
//        TextMeshProUGUI Type;

//        // Start is called before the first frame update
//        void Start()
//        {
//            CR = GameObject.Find("CardDraw").GetComponent<CardRare>();
//            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
//            //PauseUI.SetActive(true);

//            Form.sprite = CR.CardSprite;
//            RarityColor.color = CR.RarityColor;
//            Name.text = CR.Name;
//            Type.text = CR.Type;

//            anim = GetComponent<Animator>();
//            anim.SetBool("CardInterview", true);

//            StartCoroutine(PauseGame());
//            //Time.timeScale = 0.0f;
//        }

//        // 點選卡牌結束預覽畫面
//        public void InterViewDone()
//        {
//            //Instantiate(CardInform, gameObject.transform);
//            Time.timeScale = 1.0f;
//            GameObject Preview = Instantiate(Card, canvas.transform);
//            RectTransform rect = Preview.GetComponent<RectTransform>();
//            rect.anchoredPosition = new Vector3(-493f + (HandManager.Hand.IndexOf(Preview) * 150f), -250f, 0f);

//            HandManager.Hand.Add(Preview);

//            anim.SetBool("CardInterview", false);            
//            Destroy(gameObject);
//        }
        
//        //過度動畫完暫停
//        IEnumerator PauseGame()
//        {
//            yield return new WaitForSeconds(0.5f);
//            Time.timeScale = 0.0f;
//        }
//    }
//}
