using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using FusionUtilsEvents;
using UnityEngine;

namespace BlastDash
{
    public class LevelManager : NetworkBehaviour
    {
        private AudioManager audioManager;
        
        void Start()
        {
            audioManager = AudioManager.Instance;
            audioManager.backgroundMusic.clip = AudioManager.Instance.audioClips.backgroundGamePlay;
            audioManager.backgroundMusic.volume = 0.5f;
            audioManager.backgroundMusic.Play();
        }

        public override void Spawned()
        {
            FindObjectOfType<PlayerSpawner>().ReSpawnPlayers(Runner);
        }
    }
}