using System;
using UnityEngine;
using UnityEngine.Events;

namespace _local.Scripts
{
    public class DismissUI : MonoBehaviour
    {
        public string buttonToDismiss = "Submit";
        public UnityEvent OnUIDismiss;
        private void Update()
        {
            if (Input.GetButtonDown(buttonToDismiss))
            {
                OnUIDismiss?.Invoke();
            }
        }
    }
}