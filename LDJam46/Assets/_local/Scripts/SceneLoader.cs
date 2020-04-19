using UnityEngine;
using UnityEngine.SceneManagement;

namespace _local.Scripts
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(string name)
        {
            print($"Loading scene {name}");
            SceneManager.LoadScene(name);
        }
    }
}