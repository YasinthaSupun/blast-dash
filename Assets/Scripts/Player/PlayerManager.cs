using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace BlastDash
{
    public class PlayerManager : NetworkBehaviour
    {
        [Networked]
        public NetworkString<_16> PlayerName { get; set; }
        
        [Networked]
        public NetworkBool InputsAllowed { get; set; }

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                CameraController cameraController = FindObjectOfType<CameraController>();
                cameraController.SetCameraPlayer(this.transform);
            }
        }

        // public void SetInputsAllowed(bool value)
        // {
        //     InputsAllowed = value;
        // }

    }
}