using RiverCrab;
using System;
using UnityEngine;

namespace RiverCrab
{
    public class DestroyListener : MonoBehaviour
    {
        public EnemyRandomSpawn ERS;        

        private void OnDestroy()
        {
            ERS.RemoveListElement(gameObject);
        }        
    }
}
