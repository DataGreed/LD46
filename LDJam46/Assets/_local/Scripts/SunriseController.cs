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
            
        }
    }
}