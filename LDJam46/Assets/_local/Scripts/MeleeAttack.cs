using System;
using UnityEngine;

namespace _local.Scripts
{
    public class MeleeAttack : MonoBehaviour
    {
        public string tagToDamage;
        public int damagePerHit=1;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(tagToDamage)) return;
            
            var hitBox = other.GetComponent<HitBox>();
            hitBox.livingBeing.TakeDamage(damagePerHit);
            print($"Attacked {other.gameObject} for {damagePerHit} damage");
        }
    }
}