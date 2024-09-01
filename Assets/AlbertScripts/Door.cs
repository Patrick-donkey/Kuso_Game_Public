using UnityEngine;

namespace Albert
{
    public class Door : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) { OpenDoor(); }                
        }       

        private void OpenDoor()
        {
            gameObject.SetActive(false);
        }
    }

}
