using System;
using UnityEngine;

namespace _local.Scripts
{
    public class HUDController : MonoBehaviour
    {
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
            
        }
    }
}