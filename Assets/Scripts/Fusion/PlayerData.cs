using System.Collections;
using System.Collections.Generic;
using Fusion;
using FusionUtilsEvents;
using UnityEngine;

namespace BlastDash
{
    public class PlayerData : NetworkBehaviour
    {
        [Networked]
        public NetworkString<_16> Name { get; set; }
        
        public FusionEvent OnPlayerDataSpawnedEvent;
        private ChangeDetector changeDetector;

        
        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        private void RPC_SetPlayerName(string name)
        {
            Name = name;
        }
        
        public override void Spawned()
        {
            changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState, false);
            
            if (Object.HasInputAuthority)
            {
                RPC_SetPlayerName(PlayerPrefs.GetString(Utils.PlayerNamePref));
            }
            
            DontDestroyOnLoad(this);
            Runner.SetPlayerObject(Object.InputAuthority, Object);
            OnPlayerDataSpawnedEvent?.Raise(Object.InputAuthority, Runner);
            
            if (Object.HasStateAuthority)
            {
                GameManager.Instance.SetPlayerDataObject(Object.InputAuthority, this);
            }
        }
        
        public override void Render()
        {
            foreach (var change in changeDetector.DetectChanges(this))
            {
                switch (change)
                {
                    case nameof(Name):
                        OnPlayerDataSpawnedEvent?.Raise(Object.InputAuthority, Runner);
                        break;
                }
            }
        }
    }
}