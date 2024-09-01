using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Pause : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField, BoxGroup("暫停呼叫物件")]
    GameObject pauseUi;

    [SerializeField, BoxGroup("載入呼叫物件")]
    float duration;
    [SerializeField, BoxGroup("載入物件呼叫")]
    float interval;
    [SerializeField, BoxGroup("載入物件呼叫")]
    GameObject loadingtext;
    [SerializeField, BoxGroup("載入呼叫物件")]
    GameObject loadingScreen;    
    [SerializeField, BoxGroup("載入呼叫物件")]
    Image loadScreen;
    [SerializeField, BoxGroup("載入呼叫物件")]
    TextMeshProUGUI loadingText;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) PauseGame();
    }

    void PauseGame()
    {
        Time.timeScale = 0.0f;
        pauseUi.SetActive(true);
    }

    public void ContinueGame()
    {
       pauseUi.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void BackMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
        #region 加載效果
        //AsyncOperation operation = SceneManager.LoadSceneAsync("Menu");
        ////loadingScreen.SetActive(true);
        ////StartCoroutine(LoadingScreen());
        ////StartCoroutine(LoadingText(operation));
        #endregion
    }

    //IEnumerator LoadingScreen()
    //{        
    //    float elapsed = 0f;

    //    while (elapsed < duration)
    //    {
    //        elapsed += Time.deltaTime;
    //        float alpha = Mathf.Clamp01(elapsed / duration);

    //        for (int x = 0; x < loadScreen.sprite.texture.width; x++)
    //        {
    //            float progress = (float)x / loadScreen.sprite.texture.width;
    //            float newAlpha = Mathf.Clamp01(alpha * progress);

    //            loadScreen.canvasRenderer.SetAlpha(newAlpha);
    //        }

    //        yield return null;
    //    }
    //}

    //IEnumerator LoadingText(AsyncOperation operation)
    //{
    //    loadingtext.SetActive(true);
    //    yield return new WaitForSeconds(0.1f);

    //    int dotCount = 0;

    //    while (!operation.isDone)
    //    {
    //        string baseText = "Loading";            
    //        string dots = new string('.', dotCount);

    //        loadingText.text = baseText + dots;
    //        dotCount = (dotCount + 1) % 4;

    //        if (dotCount == 0)
    //        {
    //            loadingText.text = baseText;
    //        }

    //        yield return new WaitForSeconds(interval);
    //    }
    //  operation.allowSceneActivation = true;
    //}
}
