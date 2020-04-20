using System;
using UnityEngine;
using UnityEngine.Events;

namespace _local.Scripts
{
    public class SunriseController : MonoBehaviour
    {
        public float maxSecondsTillSunrise = 360;
        public float secondsTillSunrise { get; private set; } = 360;
        public bool sunRose { get; private set; } = false;
        public UnityEvent OnSunRise = new UnityEvent();

        public float sunriseSoonAlertSeconds = 100f;
        
        private bool halfwaAlerted = false;
        private bool sunriseSoonAlerted = false;
        
        private void Awake()
        {
            secondsTillSunrise = maxSecondsTillSunrise;
        }

        private void Update()
        {
            if (secondsTillSunrise < 0)
            {
                if (!sunRose)
                {
                    sunRose = true;
                    OnSunRise?.Invoke();
                }
                
                return;
            }
            
            secondsTillSunrise -= Time.deltaTime;

            if (!halfwaAlerted)
            {
                if (secondsTillSunrise < maxSecondsTillSunrise / 2)
                {
                    HUDController.Alert("I've made it halfway through the night.", 10f);
                    halfwaAlerted = true;
                }
            }

            if (!sunriseSoonAlerted)
            {
                if (secondsTillSunrise < sunriseSoonAlertSeconds)
                {
                    HUDController.Alert("Sunrise is soon, I have to keep the fire for a little longer.", 10f);
                    sunriseSoonAlerted = true;
                }
            }
            
        }
    }
}