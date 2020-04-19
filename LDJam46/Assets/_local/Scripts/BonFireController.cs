using System;
using UnityEngine;
using UnityEngine.Events;

namespace _local.Scripts
{
    public class BonFireController : MonoBehaviour
    {
        public float maxSecondsToBurn=60f;
        public UnityEvent onBurnedDown = new UnityEvent();
        private bool burnedDown=false;
        
        public float secondsLeftToBurn { get; private set; }

        private void Awake()
        {
            secondsLeftToBurn = maxSecondsToBurn;
        }

        private void OnTriggerEnter(Collider other)
        {
            print($"Bonfire entered trigger with {other}");
            
            //TODO: get wood from player inventory and add time to fire
            
            if(other.CompareTag("Player"))
            {
                var secondsToAdd = other.gameObject.GetComponent<Inventory>().FeedFire();

                if (secondsToAdd > 0)
                {
                    Feed(secondsToAdd);
                }
            }
        }

        public void Feed(int seconds)
        {
            secondsLeftToBurn += seconds;
            if (secondsLeftToBurn > maxSecondsToBurn)
            {
                secondsLeftToBurn = maxSecondsToBurn;
            }
            
            print($"Fed the fire. {secondsLeftToBurn} seconds left");
        }
        
        public void Update()
        {
            if (secondsLeftToBurn < 0)
            {
                if (!burnedDown)
                {
                    burnedDown = true;
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