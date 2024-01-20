using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace BlastDash
{
    [System.Flags]
    public enum InputButton
    {
        LEFT,
        RIGHT,
        SHOOT,
        JUMP,
    }
    
    public struct InputData : INetworkInput
    {
        public NetworkButtons buttons;

        public bool GetButton(InputButton button)
        {
            return buttons.IsSet(button);
        }
        
        public NetworkButtons GetButtonPressed(NetworkButtons prev)
        {
            return buttons.GetPressed(prev);
        }
    }
}