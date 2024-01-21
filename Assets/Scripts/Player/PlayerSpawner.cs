using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace BlastDash
{
    public class PlayerSpawner : MonoBehaviour
    {
        public NetworkPrefabRef playerPrefab;
        public Transform[] spawnPoints;
        
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

        public Vector3 GetRandomSpawnPoint()
        {
            int index = Random.Range(0, spawnPoints.Length);
            return spawnPoints[index].position;
        }
        
        private void SpawnPlayer(NetworkRunner runner, PlayerRef player, string nick = "")
        {
            if (runner.IsServer)
            {
                NetworkObject playerObj = runner.Spawn(playerPrefab,  GetRandomSpawnPoint(), Quaternion.identity, player);
            }
        }
    }
}