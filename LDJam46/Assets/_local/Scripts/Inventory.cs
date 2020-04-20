using System;
using UnityEngine;

namespace _local.Scripts
{
    public class Inventory : MonoBehaviour
    {
        public uint maxCapacity = 3;

        public int secondsPerWood = 30; 
        public int woodCarrying { get; private set; }

        public AudioSource pickupSourceSound;

        public GameObject lastTakenItem;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Item"))
            {
                print("entered item trigger");
                TryTakeItem(other.gameObject);
            }
        }

        public void TryTakeItem(GameObject item)
        {
            //safeguard so we won;t pick item twice accidentally
            //TODO: refactor: the item should have picked up property and it should have picking up logic, not inventory
            if (item==lastTakenItem) return;
            
            if (woodCarrying < maxCapacity)
            {
                //mark so we won't accidentally pick it up twice
                lastTakenItem = item;
                
                woodCarrying++;
                Destroy(item);
                print($"Picked up wood. Current wood: {woodCarrying}");
                HUDController.Alert("Picked up some wood. Can use it to light a torch or feed the campfire.", 
                    6f);
                
                pickupSourceSound.Play();

                if (woodCarrying >= maxCapacity)
                {
                    HUDController.Alert("Backpack is full, I have to find my way back to the campfire.", 10f);
                }
            }
            else
            {
                HUDController.Alert("Cannot carry any more wood. Have to find my way back to the campfire");
            }
        }

        /*
         * Removes all wood from inventory
         * and returns number of seconds to be added to fuel fire
         */
        public int FeedFire()
        {
            int woodToFeed = woodCarrying;

            woodCarrying = 0;


            int secondsToFeed = woodToFeed * secondsPerWood;

            if (secondsToFeed > 0)
            {
                print($"Feeding fire {woodToFeed} wood for {secondsToFeed} additional seconds");
            }

            return secondsToFeed;
        }

        public void BurnOneWood()
        {
            if (woodCarrying > 0)
            {
                woodCarrying--;
                
                print($"Current wood: {woodCarrying}");
            }
        }
    }
}