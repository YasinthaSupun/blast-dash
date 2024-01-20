using System.Collections;
using System.Collections.Generic;
using Fusion;
using FusionUtilsEvents;
using UnityEngine;

namespace BlastDash
{
    public class LevelManager : NetworkBehaviour
    {
        public override void Spawned()
        {
            FindObjectOfType<PlayerSpawner>().ReSpawnPlayers(Runner);
        }
    }
}