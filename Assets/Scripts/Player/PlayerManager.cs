using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Addons.Physics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlastDash
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private TMP_Text playerNameUI;
        [SerializeField] private Slider healthUI;
        [SerializeField] private PlayerAnimation animation;
        
        [Networked]
        public NetworkString<_16> PlayerName { get; set; }
        
        [Networked]
        public int Health { get; set; }
        
        [Networked]
        private NetworkBool Respawning { get; set; }
        
        [Networked]
        private TickTimer RespawnTimer { get; set; }
        
        private ChangeDetector changeDetector;
        private NetworkRigidbody2D rb;

        private void Awake()
        {
            rb = GetBehaviour<NetworkRigidbody2D>();
        }

        public override void FixedUpdateNetwork()
        {
            if (Respawning)
            {
                if (RespawnTimer.Expired(Runner))
                {
                    rb.Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    StartCoroutine(Respawn());
                }
            }
        }

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
        private void RPC_SetPlayerName(string name)
        {
            PlayerName = name;
        }
        
        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        private void RPC_SetPlayerHealth(int health)
        {
            Health = health;
        }

        private IEnumerator Respawn()
        {
            rb.Teleport(FindObjectOfType<PlayerSpawner>().GetRandomSpawnPoint());
            yield return new WaitForSeconds(.1f);
            Respawning = false;
            if (Object.HasInputAuthority)
            {
                healthUI.value = Utils.PlayerHealthMax;
                RPC_SetPlayerHealth(Utils.PlayerHealthMax);
            }
            animation.Respawn();
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == Utils.FireballTag)
            {
                Health -= Utils.PlayerHealthReduction;
                if (Health == 0)
                {
                    AudioManager.Instance.soundEffect1.PlayOneShot(AudioManager.Instance.audioClips.die);
                    rb.Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                    animation.PlayerDeath();
                    RespawnTimer = TickTimer.CreateFromSeconds(Runner, 1f);
                    Respawning = true;
                }
                else
                {
                    if (Object.HasInputAuthority)
                    {
                        RPC_SetPlayerHealth(Health);
                    }
                }
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