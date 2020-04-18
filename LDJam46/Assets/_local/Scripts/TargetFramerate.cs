using System;
using UnityEngine;

namespace _local.Scripts
{
    public class TargetFramerate : MonoBehaviour
    {
        public int framerate=60;
        
        private void Awake()
        {
            Application.targetFrameRate = framerate;
        }
    }
}