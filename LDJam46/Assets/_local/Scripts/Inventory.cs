using System;
using UnityEngine;

namespace _local.Scripts
{
    public class Inventory : MonoBehaviour
    {
        public uint maxCapacity = 3;

        public int secondsPerWood = 30; 
        public int woodCarrying { get; private set; }

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
            if (woodCarrying < maxCapacity)
            {
                woodCarrying++;
                Destroy(item);
                print($"Picked up wood. Current wood: {woodCarrying}");
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