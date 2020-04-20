using System.Collections;
using UnityEngine;

namespace _local.Scripts
{
    public class FlashSprite : MonoBehaviour
    {
        public SpriteRenderer spriteToFlash;
        public Color flashColor;
        public int timesToFlash = 1;
        public float intervalSeconds = 0.5f;
        private Color originalColor;
        
        public void Flash()
        {
            originalColor = spriteToFlash.color;
            StartCoroutine(FlashRoutine());
        }
        
        IEnumerator FlashRoutine() {

            for (int i = 0; i < timesToFlash; i++)
            {
                spriteToFlash.color = flashColor;
                yield return new WaitForSeconds(intervalSeconds);
                spriteToFlash.color = originalColor;
                yield return new WaitForSeconds(intervalSeconds);
            }

        }

    }
}