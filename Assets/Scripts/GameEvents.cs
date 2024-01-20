using System;
using UnityEngine;

namespace BlastDash
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Instance { get; private set; }
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
        
        
        // public event Action onPlayersSpawned;
        //
        // public void OnPlayersSpawned()
        // {
        //     if (onPlayersSpawned != null)
        //     {
        //         onPlayersSpawned();
        //     }
        // }
    }
}
