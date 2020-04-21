using System;
using firewalk;
using UnityEngine;
using UnityEngine.UI;

namespace _local.Scripts
{
    public class HUDController : MonoBehaviour
    {

        public static HUDController instance;
        
        [Header("Links to scene objects")]
        public LivingBeing playerLivingBeing;
        public BonFireController bonFireController;
        public Inventory inventory;
        public SunriseController sunriseController;

        [Header("Links to HUD scene objects")] 
        public GameObject heartsContainer;
        public GameObject itemsContainer;
        public GameObject alertsContainer;
        public Image bonfireTimer;
        public Image sunriseTimer;

        [Header("Links to HUD scene prefabs")] 
        public GameObject heartPrefab;
        public GameObject woodPrefab;
        public GameObject alertPrefab;

        private int previousHealth;
        private int previousWood;

        private void Awake()
        {
            instance = this;
        }

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
            
            sunriseTimer.fillAmount = 1-sunriseController.secondsTillSunrise / sunriseController.maxSecondsTillSunrise;

            if (playerLivingBeing.health != previousHealth)
            {
                previousHealth = playerLivingBeing.health;
                
                foreach(Transform child in heartsContainer.transform)
                {
                    Destroy(child.gameObject);
                }

                for (int i = 0; i < playerLivingBeing.health; i++)
                {
                    var heart = Instantiate(heartPrefab, heartsContainer.transform, true);
                    heart.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
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
                    var wood = Instantiate(woodPrefab, itemsContainer.transform, true);
                    //fix weird sizing in built versions of game
                    wood.transform.localScale = new Vector3(1,1,1);
                }
                
                print("HUD: items redrawn");
            }
        }

        /*
         * Shows short message alert to the player
         */
        public static void Alert(string message, float? alertSeconds=null)
        {
            var alert = Instantiate(instance.alertPrefab, instance.alertsContainer.transform, true);
            alert.GetComponent<Text>().text = message;
            alert.transform.localScale = new Vector3(1,1,1);

            if (alertSeconds != null)
            {
                // print("resetting timer");
                //if seconds passed, set timer
                alert.GetComponent<SelfDestruct>().ResetTimer((float)alertSeconds);
            }
        }
    }
}