using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using FusionUtilsEvents;
using TMPro;
using UnityEngine;

namespace BlastDash
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject initGamePanel;
        [SerializeField] private GameObject lobbyPanel;
        [SerializeField] private GameObject playerUI;
        [SerializeField] private Transform content;
        [SerializeField] private Transform startButton;
        [SerializeField] private TMP_InputField roomID;
        [SerializeField] private TMP_InputField name;
        
        [SerializeField] private FusionEvent OnPlayerJoinedEvent;
        [SerializeField] private FusionEvent OnPlayerDataSpawnedEvent;

        private string playerName = "Player";
        
        private void OnEnable()
        {
            OnPlayerJoinedEvent.RegisterResponse(ShowLobbyPanel);
            OnPlayerDataSpawnedEvent.RegisterResponse(UpdateLobbyList);
        }

        private void OnDisable()
        {
            OnPlayerJoinedEvent.RemoveResponse(ShowLobbyPanel);
            OnPlayerDataSpawnedEvent.RemoveResponse(UpdateLobbyList);
        }

        private void Start()
        {
            initGamePanel.SetActive(true);
            lobbyPanel.SetActive(false);
        }

        public void CreateRoom()
        {
            SetPlayerName();
            FusionLauncher Launcher = FindObjectOfType<FusionLauncher>();
            Launcher.Launch(GameMode.Host, roomID.text);
        }

        public void JoinRoom()
        {
            SetPlayerName();
            FusionLauncher Launcher = FindObjectOfType<FusionLauncher>();
            Launcher.Launch(GameMode.Client, roomID.text);
        }
        
        public void StartGame()
        {
            FusionHelper.LocalRunner.SessionInfo.IsOpen = false;
            FusionHelper.LocalRunner.SessionInfo.IsVisible = false;
            NavigationManager.Instance.LoadLevel(FusionHelper.LocalRunner);
        }
        
        private void SetPlayerName()
        {
            playerName = name.text;
            PlayerPrefs.SetString(Utils.PlayerNamePref, playerName);
        }

        private void ShowLobbyPanel(PlayerRef player, NetworkRunner runner)
        {
            initGamePanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }
        
        private void UpdateLobbyList(PlayerRef playerRef, NetworkRunner runner)
        {
            startButton.gameObject.SetActive(runner.IsServer);
            string players = default;
            string isLocal;
            ClearContentList();
            foreach(var player in runner.ActivePlayers)
            {
                isLocal = player == runner.LocalPlayer ? " (You)" : string.Empty;
                string currentPlayerName = GameManager.Instance.GetPlayerData(player, runner)?.Name + isLocal ;
                Debug.Log("zzzz"+currentPlayerName);
                GameObject playerUIObj = Instantiate(playerUI, content);
                playerUIObj.GetComponent<PlayerUI>().SetPlayerName(currentPlayerName);
            }
            
            // _lobbyRoomName.text = $"Room: {runner.SessionInfo.Name}";
        }

        private void ClearContentList()
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
        }
    }
}