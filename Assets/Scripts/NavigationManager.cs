using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using Fusion;

namespace BlastDash
{
    public class NavigationManager : MonoBehaviour
    {
        public static NavigationManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        public void LoadLevel(NetworkRunner runner)
        {
            string scenePath = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1));
            runner.LoadScene(scenePath);
        }
        
        public async void LoadScene(SceneName scene)
        {
            await LoadSceneAsync(scene, LoadSceneMode.Single);
        }
        
        private async UniTask LoadSceneAsync(SceneName scene, LoadSceneMode mode)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene.ToString(), mode);
            if (asyncOperation != null)
            {
                await asyncOperation;
            }
        }
    }
}