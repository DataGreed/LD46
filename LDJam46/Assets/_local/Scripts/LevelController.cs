using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _local.Scripts
{
    public class LevelController : MonoBehaviour
    {

        public SunriseController sunriseController;
        public BonFireController bonfireController;
        
        public bool won { get; private set; }
        public bool lost { get; private set; }

        public void Update()
        {
            if (Time.frameCount / 4 == 0)
            {
                if (!won)
                {
                    CheckVictoryCondition();
                }
            }
        }

        private void CheckVictoryCondition()
        {
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
            }
        }
        
        public void GameOverFire()
        {
            if (!lost)
            {
                lost = true;
                print("Game Over - bonfire went out");
            }
        }

        public void RestartLevel()
        {
            print("Restarting level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        public void Victory()
        {
            if (!won)
            {
                won = true;
                print("Game Over");
            }
        }
    }
}