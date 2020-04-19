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

        [Header("Links to HUD scene objects")] 
        public GameObject heartsContainer;
        public GameObject itemsContainer;
        public Image bonfireTimer;

        [Header("Links to HUD scene prefabs")] 
        public GameObject heart;
        public GameObject wood;
        
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
        }
    }
}