using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    bool isportal;
    [SerializeField]
    GameObject Scene;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) isportal = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) isportal = false;
    }

    private void Update()
    {
        if (isportal && Input.GetKeyDown(KeyCode.E))
        {
            GM.freeze = true;
            Scene.SetActive(true);
        }
        else if (isportal && Input.GetKeyDown(KeyCode.Escape))
        {
            Scene.SetActive(false);
            GM.freeze = false;
        }
    }
    public void Transport(int Index)
    {
        GM.freeze = false;
        SceneManager.LoadScene(Index);        
    }
}
