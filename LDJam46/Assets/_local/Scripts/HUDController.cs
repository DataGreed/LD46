using System;
using firewalk;
using UnityEngine;
using UnityEngine.UI;

namespace _local.Scripts
{
    public class HUDController : MonoBehaviour
    {

        [Header("Links to scene objects")]
        public LivingBeing playerLivingBeing;
        public BonFireController bonFireController;
        public Inventory inventory;

        [Header("Links to HUD scene objects")] 
        public GameObject heartsContainer;
        public GameObject itemsContainer;
        public Image bonfireTimer;

        [Header("Links to HUD scene prefabs")] 
        public GameObject heartPrefab;
        public GameObject woodPrefab;

        private int previousHealth;
        private int previousWood;
        
        private void Start()
        {
            Redraw();
        }

        private void Update()
        {
            if (Time.frameCount % 4 == 0) //redraw every 4 frames
            {
                Redraw();
            }
        }

        void Redraw()
        {
            //TODO: add smooth animation
            bonfireTimer.fillAmount = bonFireController.secondsLeftToBurn / bonFireController.maxSecondsToBurn;

            if (playerLivingBeing.health != previousHealth)
            {
                previousHealth = playerLivingBeing.health;
                
                foreach(Transform child in heartsContainer.transform)
                {
                    Destroy(child.gameObject);
                }

                for (int i = 0; i < playerLivingBeing.health; i++)
                {
                    var heart = Instantiate(heartPrefab);
                    heart.transform.SetParent(heartsContainer.transform);
                }
                
                print("HUD: health redrawn");
            }
            
            if (inventory.woodCarrying != previousWood)
            {
                previousWood = inventory.woodCarrying;
                
                foreach(Transform child in itemsContainer.transform)
                {
                    Destroy(child.gameObject);
                }

                for (int i = 0; i < inventory.woodCarrying; i++)
                {
                    var heart = Instantiate(woodPrefab);
                    heart.transform.SetParent(itemsContainer.transform);
                }
                
                print("HUD: items redrawn");
            }
        }
    }
}