using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastDash
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform player;

        private void Update()
        {
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        }
    }
}