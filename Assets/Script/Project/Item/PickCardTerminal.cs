using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickCardTerminal : MonoBehaviour
{
    bool isPickUI;
    [SerializeField]
    GameObject PickUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) isPickUI = true;        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) isPickUI = false;
    }

    private void Update()
    {
        if (isPickUI&&Input.GetKeyDown(KeyCode.E))
        {
            GM.freeze = true;
            PickUI.SetActive(true);
        }
        else if (isPickUI && Input.GetKeyDown(KeyCode.Escape))
        {
            PickUI.SetActive(false);
            GM.freeze = false;
        }
    }
}
