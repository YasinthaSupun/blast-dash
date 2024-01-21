using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

namespace BlastDash
{
    [RequireComponent(typeof(NetworkRunner))]
    public class FusionLauncher : MonoBehaviour
    {
        private NetworkRunner networkRunner;
        private FusionSceneManager fusionSceneManager;

        private void Awake()
        {
            networkRunner = GetComponent<NetworkRunner>();
            fusionSceneManager = GetComponent<FusionSceneManager>();
            
        }

        public void Launch(GameMode mode, string room)
        {
            networkRunner.name = name;
            networkRunner.ProvideInput = true;
            
            networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = room,
                SceneManager = fusionSceneManager
            });
        }
    }
}