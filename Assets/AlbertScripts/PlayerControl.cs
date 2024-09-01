using Fungus;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Albert
{
    public class PlayerControl : MonoBehaviour
    {
        #region 屬性區
        [SerializeField, LabelText("出生地")] private Transform spawnPlace;
        #endregion

        #region 事件
        private void Start()
        {
            transform.position = spawnPlace.transform.position;
        }


        private void Update()
        {

        }


        private void FixedUpdate()
        {

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger)
                print("TRIGGER");
            //collision.gameObject.SetActive(false);
        }

        #endregion
        #region 方法區

        #endregion
    }


}

