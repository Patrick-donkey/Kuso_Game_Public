using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RiverCrab
{
    public class RewardsPreview : MonoBehaviour
    {
        // Start is called before the first frame update
        [Header("����Ѧ�")]
        Animator anim;

        [Header("����d�P�Ѽ�")]
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

        // �I��d�P�����w���e��
        public void PreviewDone()
        {
            //Instantiate(CardInform, gameObject.transform);
            Time.timeScale = 1.0f;

            anim.SetBool("CardInterview", false);
            Destroy(gameObject);
        }

        //�L�װʵe���Ȱ�
        IEnumerator PauseGame()
        {
            yield return new WaitForSeconds(0.5f);
            Time.timeScale = 0.0f;
        }
    }
}
