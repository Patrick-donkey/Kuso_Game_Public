using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace RiverCrab
{
    public class EnemyRandomSpawn : MonoBehaviour
    {
        [Header("生成位置與觸發區")]
        [SerializeField]
        Transform LeftPos;
        [SerializeField]
        Transform RightPos;
        [SerializeField]
        Transform SpawnPos;
        [SerializeField]
        BoxCollider2D boxCollider;

        [Header("儲存生成怪物與空氣牆")]
        [SerializeField]
        public List<GameObject> EnemyList = new List<GameObject>();
        [SerializeField]
        GameObject[] WallObj;
        public GameObject Self;

        //事件參數
        public event EventHandler ListCotroller;       

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() == "UnityEngine.BoxCollider2D")
            {
                boxCollider.enabled = false;
                for(int i = 0; i<WallObj.Length; i++)
                {
                    WallObj[i].SetActive(true);
                }

                for(int i = 0; i < EnemyList.Count; i++)
                {
                    GameObject enemy = Instantiate(EnemyList[i],SpawnPoint(),Quaternion.identity);
                    DestroyListener destroyListener = enemy.transform.GetChild(0).AddComponent<DestroyListener>();
                    destroyListener.ERS = GetComponent<EnemyRandomSpawn>();                    
                }
            }
        }

        Vector2 SpawnPoint()
        {
            SpawnPos.position =new Vector2(UnityEngine.Random.Range(LeftPos.position.x,RightPos.position.x),UnityEngine.Random.Range(LeftPos.position.y,RightPos.position.y));
            return SpawnPos.position;
        }

        public void RemoveListElement(GameObject Enemy)
        {
            EnemyList.RemoveAt(0);
            if (EnemyList.Count == 0)
            {
                Destroy(Self);
            }            
        }        
    }
}
