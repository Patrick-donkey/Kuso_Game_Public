using UnityEngine;

namespace RiverCrab
{
    public class Victory : MonoBehaviour
    {
        [SerializeField]
        GameObject[] setTrue;
        // Start is called before the first frame update
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() == "UnityEngine.BoxCollider2D")
            {
                foreach (var gObj in setTrue)
                {
                    gObj.SetActive(true);
                }
            }
        }
    }
}
