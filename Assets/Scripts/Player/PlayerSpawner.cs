using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace BlastDash
{
    public class PlayerSpawner : MonoBehaviour
    {
        public NetworkPrefabRef playerPrefab;
        [SerializeField] private Transform spawnPoint;
        
        public void ReSpawnPlayers(NetworkRunner runner)
        {
            if (!runner.IsClient)
            {
                foreach (var player in runner.ActivePlayers)
                {
                    SpawnPlayer(runner, player, GameManager.Instance.GetPlayerData(player, runner).Name.ToString());
                }
            }
        }
        
        private void SpawnPlayer(NetworkRunner runner, PlayerRef player, string nick = "")
        {
            if (runner.IsServer)
            {
                NetworkObject playerObj = runner.Spawn(playerPrefab, spawnPoint.position, Quaternion.identity, player);

                // PlayerData data = GameManager.Instance.GetPlayerData(player, runner);
                // data.Instance = playerObj;
                //
                // playerObj.GetComponent<PlayerBehaviour>().Nickname = data.Nick;
            }
        }
    }
}