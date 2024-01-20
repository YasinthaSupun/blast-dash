using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlastDash
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        private Dictionary<PlayerRef, PlayerData> playerDataDic = new ();
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(this.transform.parent.gameObject);
            }
            DontDestroyOnLoad(transform.parent);
        }
        
        public PlayerData GetPlayerData(PlayerRef player, NetworkRunner runner)
        {
            NetworkObject networkObj;
            if (runner.TryGetPlayerObject(player, out networkObj))
            {
                PlayerData data = networkObj.GetComponent<PlayerData>();
                return data;
            }
            
            Debug.LogWarning("Player not found");
            return null;
        }
        
        public void SetPlayerDataObject(PlayerRef objectInputAuthority, PlayerData playerData)
        {
            playerDataDic.Add(objectInputAuthority, playerData);
        }
    }
}