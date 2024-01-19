using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlastDash
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform spawnPointsContainer;

        private void Start()
        {
            int positionIndex = Random.Range(0, spawnPointsContainer.childCount);
        }
    }
}