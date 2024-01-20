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
        
        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        public void RPC_SetPlayerName(string name)
        {
            Name = name;
        }
        
        public override void Spawned()
        {
            if (Object.HasInputAuthority)
                RPC_SetPlayerName(PlayerPrefs.GetString(Utils.PlayerNamePref));
            
            DontDestroyOnLoad(this);
            Runner.SetPlayerObject(Object.InputAuthority, Object);
            OnPlayerDataSpawnedEvent?.Raise(Object.InputAuthority, Runner);
            
            if (Object.HasStateAuthority)
            {
                GameManager.Instance.SetPlayerDataObject(Object.InputAuthority, this);
            }
        }
    }
}