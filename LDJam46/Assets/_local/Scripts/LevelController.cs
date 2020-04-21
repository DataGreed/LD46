using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace _local.Scripts
{
    public class LevelController : MonoBehaviour
    {

        public SunriseController sunriseController;
        public BonFireController bonfireController;
        
        public bool won { get; private set; }
        public bool lost { get; private set; }
        
        public UnityEvent OnLevelStart;
        public UnityEvent OnLevelWon;
        public UnityEvent OnLevelLostFire;
        public UnityEvent OnLevelLostDead;
        public UnityEvent OnLevelLost;

        private void Start()
        {
            OnLevelStart?.Invoke();
        }

        public void Update()
        {
            if (Time.frameCount % 10 == 0)
            {
                if (!won)
                {
                    CheckVictoryCondition();
                }
            }
        }

        private void CheckVictoryCondition()
        {
            //print($"left to burn {bonfireController.secondsLeftToBurn} - sunrise in {sunriseController.secondsTillSunrise}");
            
            if (bonfireController.secondsLeftToBurn > sunriseController.secondsTillSunrise)
            {
                Victory();
            }
        }

        public void GameOverDead()
        {
            if (!lost)
            {
                lost = true;
                print("Game Over - you're dead");
                OnLevelLostDead?.Invoke();
                GameOver();
            }
        }
        
        public void GameOverFire()
        {
            if (!lost)
            {
                lost = true;
                print("Game Over - bonfire went out");
                OnLevelLostFire?.Invoke();
                GameOver();
            }
        }

        public void GameOver()
        {
            OnLevelLost?.Invoke();
        }

        public void RestartLevel()
        {
            print("Restarting level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        public void GoToTitle()
        {
            print("Going to title");
            SceneManager.LoadScene("Title");
        }
        
        public void Victory()
        {
            if (!won)
            {
                won = true;
                print("Victory!");
                OnLevelWon?.Invoke();
            }
        }
    }
}