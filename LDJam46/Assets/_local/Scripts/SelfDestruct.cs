using System;
using UnityEngine;

namespace _local.Scripts
{
    public class SelfDestruct : MonoBehaviour
    {
        public float destroyInSeconds = 3;

        private float secondsLeft=0;
        public void Start()
        {
            if (secondsLeft <= 0)
            {
                secondsLeft = destroyInSeconds;
            }//else user the ones passed with reset timer function
        }

        /*
         * Reset timer, useful when instntiating from code
         */
        public void ResetTimer(float seconds)
        {
            secondsLeft = seconds;
        }

        private void Update()
        {
            secondsLeft -= Time.deltaTime;
            if (secondsLeft < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}