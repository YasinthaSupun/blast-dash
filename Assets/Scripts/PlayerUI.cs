using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BlastDash
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerName;

        public void SetPlayerName(string name)
        {
            playerName.text = name;
        }
    }
}