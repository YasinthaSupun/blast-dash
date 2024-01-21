using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastDash
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        public AudioSource backgroundMusic;
        public AudioSource soundEffect1;
        public AudioClipsData audioClips;
        
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
        
        private void SetBackgroundVolume(float volume)
        {
            backgroundMusic.volume = volume;
        }
        
        private void SetEffectsVolume(float volume)
        {
            soundEffect1.volume = volume;
        }
    }
}
