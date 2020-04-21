using System;
using UnityEngine;
using UnityEngine.Events;

namespace pro.datagreed.performance
{
    public class FrustrumEventRaiser : MonoBehaviour
    {
        public UnityEvent BecameVisible = new UnityEvent();
        public UnityEvent BecameInvisible = new UnityEvent();

        public void Start()
        {
            BecameInvisible?.Invoke();
        }

        private void OnBecameInvisible()
        {
            // Debug.Log("invisible", this);
            BecameInvisible?.Invoke();
        }
        
        private void OnBecameVisible()
        {
            // Debug.Log("visible", this);
            BecameVisible?.Invoke();
        }
    }
}