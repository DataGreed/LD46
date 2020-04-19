using System;
using UnityEngine;
using UnityEngine.Events;

namespace _local.Scripts
{
    public class BonFireController : MonoBehaviour
    {
        public float maxSecondsToBurn=60f;
        public UnityEvent onBurnedDown = new UnityEvent();
        private bool burnedDown;
        
        public float secondsLeftToBurn { get; private set; }

        private void Awake()
        {
            secondsLeftToBurn = maxSecondsToBurn;
        }

        private void OnTriggerEnter(Collider other)
        {
            print($"Bonfire entered trigger with {other}");
            
            //TODO: get wood from player inventory and add time to fire
        }

        public void Update()
        {
            if (secondsLeftToBurn < 0)
            {
                if (!burnedDown)
                {
                    onBurnedDown?.Invoke();
                }
            }
            else
            {
                secondsLeftToBurn -= Time.deltaTime;
            }
        }
    }
}