using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace datagreed.camera
{
    
    public class FollowCamera : MonoBehaviour
    {
        public Transform objectToFollow;
        public bool smooth;
        [Range(0, 0.8f)]
        public float smoothTime = 0.3F;
        
        private Vector3 velocity = Vector3.zero;
        private Vector3 _delta;
        
        private void Start()
        {
            _delta = transform.position - objectToFollow.position;
        }

        // Update is called once per frame
        void LateUpdate()
        {

            float x = objectToFollow.position.x + _delta.x;
            float y = objectToFollow.position.y + _delta.y;
            float z = objectToFollow.position.z + _delta.z;

            Vector3 newPosition = new Vector3(x, y, z);
            
            if (smooth)
            {
                transform.position = Vector3.SmoothDamp(transform.position, 
                    newPosition, 
                    ref velocity, 
                    smoothTime);
            }
            else
            {
                transform.position = newPosition;
            }

        }
    }
}