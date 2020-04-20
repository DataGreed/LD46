using System;
using UnityEngine;

namespace _local.Scripts
{
    public class SelfDestruct : MonoBehaviour
    {
        public float destroyInSeconds = 3;

        private float secondsLeft;
        public void Start()
        {
            secondsLeft = destroyInSeconds;
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