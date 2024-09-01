using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace RiverCrab
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField]
        GameObject[] setFalse;
        [SerializeField]
        TextMeshProUGUI tmp;
        [SerializeField]
        GameObject Button;
        // Start is called before the first frame update
        void Start()
        {
            foreach (var item in setFalse)
            {
                item.SetActive(false);
            }
            StartCoroutine(GameOverTxt());
        }

        public void StartNew()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        IEnumerator GameOverTxt()
        {
            Color current = new Color(1f, 1f, 1f, 0f);
            for (float i = 0; i < 1; i += 0.05f)
            {
                yield return new WaitForSeconds(0.1f);
                current = new Color(1, 1, 1, i);
                tmp.color = current;
            }
            yield return new WaitForSeconds(1f);
            Button.SetActive(true);
        }
    }
}
