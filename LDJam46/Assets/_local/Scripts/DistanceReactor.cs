using System;
using MyNamespace;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace pro.datagreed.performance
{
    public class DistanceReactor : MonoBehaviour
    {
        public float distanceToTrigger = 20f;
        public int checkEveryFrames = 60;
        
        [Tooltip("Makes all instances of DistanceReactor do checks " +
                 "not at the same time but distributed more evenly in time to " +
                 "avoid peaks that may slow down performance")]
        public bool randomizeFrameShift = true;
        
        public UnityEvent OnObjectInRange = new UnityEvent();
        public UnityEvent OnObjectOutOfRange = new UnityEvent();

        private int frameShift = 0;
        // private bool initialized = false;
        
        public bool objectWithinRange { get; private set; }
        
        private void Start()
        {
            if (randomizeFrameShift) frameShift = Random.Range(0, checkEveryFrames);
            
            
            //initialize
            //this may be heavy :(
            //TODO: probably better manually initializing objectWithinRange as true for objects 
            //that are spawned near the player at the start of the level? 
            if (Vector2.Distance(PlayerController.instance.transform.position, transform.position) >
                distanceToTrigger)
            {
                objectWithinRange = false;
                OnObjectOutOfRange?.Invoke();
            }
            else
            {
                objectWithinRange = true;
                OnObjectOutOfRange?.Invoke();
            }
        }

        private void Update()
        {
            //FIXME: something does not quite work here
            
            //FIXME: can unity skip frames? Maybe it'll be safer to use timer instead of relying on frames?
            if ((Time.frameCount + frameShift) % checkEveryFrames == 0)
            {
                
                if (objectWithinRange)    //this check prevents us from firing events more than once
                {
                    //FIXME: could calculating square distance be faster thatn usiing Distance method?
                    //FIXME: cache player transform?
                    
                    if (Vector2.Distance(PlayerController.instance.transform.position, transform.position) >
                        distanceToTrigger)
                    {
                        objectWithinRange = false;
                        OnObjectOutOfRange?.Invoke();
                    }
                }
                else
                {
                    if (Vector2.Distance(PlayerController.instance.transform.position, transform.position) <
                        distanceToTrigger)
                    {
                        objectWithinRange = true;
                        OnObjectInRange?.Invoke();
                    }
                }
            }
        }
    }
}