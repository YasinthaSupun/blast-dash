using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastDash
{
    public class LobbyController : MonoBehaviour
    {
        private AudioManager audioManager;
        private void Start()
        {
            audioManager = AudioManager.Instance;
            audioManager.backgroundMusic.clip = AudioManager.Instance.audioClips.backgroundLobby;
            audioManager.backgroundMusic.volume = 1f;
            audioManager.backgroundMusic.Play();
        }

        public void PlayButtonClick()
        {
            audioManager.soundEffect1.PlayOneShot(AudioManager.Instance.audioClips.uiClick);
        }
    }
}
