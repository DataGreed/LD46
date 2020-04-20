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
        public Fading2DLight fadingLight;
        
        public float alertAtSeconds=35f;
        public bool alertRaised = false;
        
        public float secondsLeftToBurn { get; private set; }

        private void Awake()
        {
            secondsLeftToBurn = maxSecondsToBurn;
            fadingLight = GetComponent<Fading2DLight>();
            fadingLight.secondsToFadeOut = secondsLeftToBurn;
            fadingLight.Reignite();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            print($"Bonfire entered trigger with {other}");
            
            //TODO: get wood from player inventory and add time to fire
            
            if(other.CompareTag("Player"))
            {
                var inventory = other.GetComponent<Inventory>();
                
                Debug.Log(inventory);
                
                var secondsToAdd = inventory.FeedFire();

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
            
            fadingLight.secondsToFadeOut = secondsLeftToBurn;
            fadingLight.Reignite();
            
            print($"Fed the fire. {secondsLeftToBurn} seconds left");
            HUDController.Alert("Fed the bonfire with all the wood I've found.", 6f);

            if (secondsLeftToBurn > alertAtSeconds)
            {
                //reset alert
                alertRaised = false;
            }
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

                if (secondsLeftToBurn <= alertAtSeconds && !alertRaised)
                {
                    HUDController.Alert("Campfire will go out soon. I need to feed it with some wood.", 6f);
                    alertRaised = true;
                }
            }
        }
    }
}