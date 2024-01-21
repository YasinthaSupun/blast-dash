using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlastDash
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private TMP_Text playerNameUI;
        [SerializeField] private Slider healthUI;
        
        [Networked]
        public NetworkString<_16> PlayerName { get; set; }
        
        [Networked]
        public int Health { get; set; }
        
        private ChangeDetector changeDetector;

        public override void Spawned()
        {
            changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState, false);
            
            if (Object.HasInputAuthority)
            {
                healthUI.value = Utils.PlayerHealthMax;
                CameraController cameraController = FindObjectOfType<CameraController>();
                cameraController.SetCameraPlayer(this.transform);
                
                if (PlayerName == string.Empty)
                {
                    RPC_SetPlayerName(PlayerPrefs.GetString(Utils.PlayerNamePref));
                }
                RPC_SetPlayerHealth(Utils.PlayerHealthMax);
            }

            playerNameUI.text = PlayerName.ToString();
        }

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        public void RPC_SetPlayerName(string name)
        {
            PlayerName = name;
        }
        
        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        public void RPC_SetPlayerHealth(int health)
        {
            Health = health;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == Utils.FireballTag)
            {
                Health -= 2;
                RPC_SetPlayerHealth(Health);
            }
        }

        public override void Render()
        {
            foreach (var change in changeDetector.DetectChanges(this))
            {
                switch (change)
                {
                    case nameof(PlayerName):
                        playerNameUI.text = PlayerName.ToString();
                        break;
                    case nameof(Health):
                        healthUI.value = Health;
                        break;
                }
            }
        }
    }
}