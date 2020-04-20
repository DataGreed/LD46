using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _local.Scripts
{
    public class PeriodicSoundPlayer : MonoBehaviour
    {
        public AudioClip[] audioClips;
        public float minTimeOut=1;
        public float maxTimeOut=10;

        public AudioSource audioSource;
        
        private float secondsTillNextSound;

        private void Start()
        {
            ResetTimeOut();
        }

        private void Update()
        {
            secondsTillNextSound -= Time.deltaTime;

            if (secondsTillNextSound < 0)
            {
                PlayRandomSound();
                ResetTimeOut();
            }

        }

        public void PlayRandomSound()
        {
            audioSource.PlayOneShot(audioClips[Random.Range(0,audioClips.Length-1)]);
        }

        public void ResetTimeOut()
        {
            secondsTillNextSound = Random.Range(minTimeOut, maxTimeOut);
        }
    }
}