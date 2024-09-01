using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField, LabelText("點擊音效")]
    AudioSource _clickSFX;
    Button button;
    float seconds = 3f;
    private void Start()
    {
        button = GetComponent<Button>();
        button.enabled = true;
    }
    /// <summary>
    /// 播放完音效後，進入遊戲畫面。
    /// </summary>
    public void Delay()
    {
        button.enabled = false;
        _clickSFX.Play();        
        DOVirtual.DelayedCall(seconds, LoadGame, false);               
    }

    void LoadGame()
    {
        SceneManager.LoadScene(2);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
