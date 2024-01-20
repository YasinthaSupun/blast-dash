using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BlastDash
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float damping = 10f;
        
        private bool targetSet = false;
        private Transform player;

        public void SetCameraPlayer(Transform targetTransform)
        {
            player = targetTransform;
            targetSet = true;
        }

        private void Update()
        {
            if (targetSet)
            {
                Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, damping * Time.deltaTime);
            }
        }
    }
}