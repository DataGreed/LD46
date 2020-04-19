using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

namespace _local.Scripts
{
    public class Fading2DLight : MonoBehaviour
    {
        public Light2D light;
        public float secondsToFadeOut=30f;
        public float defaultIntensity = 1;
        public float fadeOutIntensity = 0.3f;
        public bool disableOnFadedOut = true;
        public bool deactivateGameObjectOnFadeOut = false;
        public UnityEvent OnFadedOut = new UnityEvent();
        private bool fadedOut = false;
        
        private float velocity;
        
        public void Start()
        {
            if (light == null) light = GetComponent<Light2D>();
            Reignite();
        }
        public void Reignite()
        {
            light.enabled = true;
            light.intensity = defaultIntensity;
            fadedOut = false;
        }

        private void Update()
        {
            if (light.intensity > fadeOutIntensity+ 0.05f)
            {
                light.intensity=Mathf.SmoothDamp(light.intensity, fadeOutIntensity, ref velocity, secondsToFadeOut);    
            }
            else
            {
                if(!fadedOut)
                {
                    fadedOut = true;
                    
                    if (disableOnFadedOut)
                    {
                        light.enabled = false;
                    }

                    OnFadedOut?.Invoke();
                    
                    if (deactivateGameObjectOnFadeOut)
                    {
                        gameObject.SetActive(false);
                    }
                }
                
            }
        }
    }
}