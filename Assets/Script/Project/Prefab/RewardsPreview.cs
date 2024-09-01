using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RiverCrab
{
    public class RewardsPreview : MonoBehaviour
    {
        // Start is called before the first frame update
        [Header("獲取參考")]
        Animator anim;

        [Header("獲取卡牌參數")]
        [SerializeField]
        public Image Form;
        [SerializeField]
        public Image RarityColor;
        [SerializeField]
        public TextMeshProUGUI Name;
        [SerializeField]
        public TextMeshProUGUI Type;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            anim.SetBool("CardInterview", true);

            StartCoroutine(PauseGame());
            //Time.timeScale = 0.0f;
        }

        // 點選卡牌結束預覽畫面
        public void PreviewDone()
        {
            //Instantiate(CardInform, gameObject.transform);
            Time.timeScale = 1.0f;

            anim.SetBool("CardInterview", false);
            Destroy(gameObject);
        }

        //過度動畫完暫停
        IEnumerator PauseGame()
        {
            yield return new WaitForSeconds(0.5f);
            Time.timeScale = 0.0f;
        }
    }
}
