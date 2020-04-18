using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


namespace DrunkShotgun
{
    public class LivingBeing : MonoBehaviour
    {
        public uint startingHealth;
        public uint defaultRevivalHealth;
        public uint maxHealth;

        [Range(0,2)]
        [Tooltip("Become invincible for short period of time after taken damage. Set to 0 to disable")]
        public float invincibleAfterHurtTime = 0;
        [Range(0,3)]
        public float invincibleAfterRevivedTime = 0;
        
        public bool invincible = false;

        public LivingBeingDeathEvent onDeath = new LivingBeingDeathEvent();
        public LivingBeingTookDamageEvent onDamageTaken = new LivingBeingTookDamageEvent();
        public LivingBeingRevivedEvent onRevived = new LivingBeingRevivedEvent();
        
        public Animator animator;

        public string _animatorDeadBool="isDead";
        public bool setInactiveOnDeath = true;
        
        public int health { get; private set; }

        [Tooltip("Created when hit by projectile. E.g. blood splash")]
        public GameObject hitAnimationPrefab;
        
        public GameObject bloodDecalPrefab;

        [Tooltip("Created when being is revived")]
        public GameObject revivalAnimationPrefab;

        // public SoundEffectType damagedSound;
        // public bool playDamagedSound;

        private void Awake()
        {
            if(animator==null)animator = GetComponent<Animator>();
        }

        private void Start()
        {
            // validate values
            if (startingHealth <= 0) throw new Exception("Starting health must be greater than zero");
            if (defaultRevivalHealth<= 0) throw new Exception("Default revival health must be greater than zero");
            if (maxHealth < startingHealth) throw new Exception("Max Health cannot be less than Starting health");
            
            
            //initialize values
            health = (int)startingHealth;
        }

        public Boolean IsAlive()
        {
            return health > 0;
        }
        
        public void TakeDamage(int damage) //signed int so we can possibly implement healing bullets :)
        {
            if(invincible) return;

            if (!IsAlive()) return;
            
            health -= damage;
            
            onDamageTaken?.Invoke(this);

            if (!IsAlive())
            {
                Kill();
            }
            else
            {
                //make invincible for short period of time to allow escaping attack
                if (invincibleAfterHurtTime > 0)
                {
                    MakeInvincibleForSeconds(invincibleAfterHurtTime);
                }
            }
            
            //TODO: limit number of decals on the screen at the same time
            if (bloodDecalPrefab != null)
            {
                //TODO: add delay or animation so it will appear when blood particles drop on ground
                var placedDecal = Instantiate(bloodDecalPrefab, transform.position, Quaternion.Euler(0,0, Random.Range(0,359)));
                
                //randomize decal size
                float decalScale = Random.Range(0.8f, 1.2f);
                placedDecal.transform.localScale = new Vector3(decalScale,decalScale,decalScale);
            }

            // if (playDamagedSound)
            // {
            //     SoundManager.instance.PlaySound(damagedSound);
            // }
            
            //TODO: check for maximum health if healing bullet
            //TODO: blood splatter
            //TODO: damage animation
        }

        private void BecomeMortal()
        {
            invincible = false;
        }
        
        public void Heal(uint healPoints)
        {
            //can only heal alive creatures
            if (!IsAlive()) return;
            
            if (health + healPoints >= maxHealth)
            {
                health = (int)maxHealth;
            }
            else
            {
                health += (int)maxHealth;
            }
        }

        /// <summary>
        /// Revives dead creature
        /// </summary>
        /// <param name="reviveHealthPoints">Number of health points the creature will be revived with</param>
        public void Revive(uint reviveHealthPoints)
        {
            //can only revive killed creatures
            if(IsAlive()) return;
            
            //make invincible for short period of time
            if(invincibleAfterRevivedTime>0) MakeInvincibleForSeconds(invincibleAfterRevivedTime);
            
            health = reviveHealthPoints >= maxHealth ? (int)maxHealth : (int)reviveHealthPoints;
            
            animator.SetBool(_animatorDeadBool, false);
            
            //create revival animation object if not null
            if (revivalAnimationPrefab != null)
            {
                var revivalAnimation = Instantiate(revivalAnimationPrefab, transform.position, transform.rotation);
            }
            
            onRevived?.Invoke(this);
            
            //activate if not active
            if(!gameObject.activeSelf)gameObject.SetActive(true);
        }

        public void Revive()
        {
            Revive(defaultRevivalHealth);
        }

        /// <summary>
        /// Makes being temporarily invincible. Useful after getting hurt
        /// Does not stack, overrides previous call
        /// </summary>
        /// <param name="seconds"></param>
        public void MakeInvincibleForSeconds(float seconds)
        {
            //exit if being permanently invincible
//            if(invincible && !IsInvoking(nameof(BecomeMortal))) return;
            
            if (IsInvoking(nameof(BecomeMortal)))
            {
                CancelInvoke(nameof(BecomeMortal));
            }
            invincible = true;
            Invoke(nameof(BecomeMortal), seconds);
        }
        
        private void Kill()
        {
            //TODO: death animation
            //TODO: something should remain lying around without collider

            if(invincible) return;
            
            //animate if not set to disable on death
            if (!setInactiveOnDeath)
            {
                animator.SetBool(_animatorDeadBool, true);
            }

            onDeath?.Invoke(this);
            
            //TODO: optimize: this may clutter the memory. E.g. pool
            if (setInactiveOnDeath)
            {
                gameObject.SetActive(false);
            }
//            Destroy(gameObject);
            
        }
    }

    [Serializable]
    public class LivingBeingDeathEvent : UnityEvent<LivingBeing>
    {
    }
    
    [Serializable]
    public class LivingBeingTookDamageEvent : UnityEvent<LivingBeing>
    {
    }
    
    public class LivingBeingRevivedEvent : UnityEvent<LivingBeing>
    {
    }
}
