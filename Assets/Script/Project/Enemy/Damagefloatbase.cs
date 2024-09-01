using UnityEngine;

namespace RiverCrab
{
    public class Damagefloatbase : MonoBehaviour
    {
        [SerializeField, Range(0, 1)]
        float destime;

        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, destime);
        }
    }
}
