using UnityEngine;
using UnityEngine.Events;

namespace _local.Scripts
{
    
    public class AnimationEventRaiser : MonoBehaviour
    {
        public UnityEvent onEvadeEnded = new UnityEvent();
        
        public void EvadeEnded()
        {
            print("Raising event onEvadeEnded from animator");
            onEvadeEnded?.Invoke();
        }
    }
}