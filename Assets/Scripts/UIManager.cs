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
        [SerializeField] private TMP_InputField createRoomID;
        [SerializeField] private TMP_InputField joinRoomID;
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
            playerName = name.text;
            PlayerPrefs.SetString(Utils.PlayerNamePref, playerName);
            FusionLauncher Launcher = FindObjectOfType<FusionLauncher>();
            Launcher.Launch(GameMode.Host, createRoomID.text);
        }

        public void JoinRoom()
        {
            playerName = name.text;
            PlayerPrefs.SetString(Utils.PlayerNamePref, playerName);
            FusionLauncher Launcher = FindObjectOfType<FusionLauncher>();
            Launcher.Launch(GameMode.Client, joinRoomID.text);
        }
        
        public void StartGame()
        {
            FusionHelper.LocalRunner.SessionInfo.IsOpen = false;
            FusionHelper.LocalRunner.SessionInfo.IsVisible = false;
            NavigationManager.Instance.LoadLevel(FusionHelper.LocalRunner);
        }

        private void ShowLobbyPanel(PlayerRef player, NetworkRunner runner)
        {
            initGamePanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }
        
        private void UpdateLobbyList(PlayerRef playerRef, NetworkRunner runner)
        {
            //_startButton.gameObject.SetActive(runner.IsServer);
            string players = default;
            string isLocal;
            foreach(var player in runner.ActivePlayers)
            {
                isLocal = player == runner.LocalPlayer ? " (You)" : string.Empty;
                players += GameManager.Instance.GetPlayerData(player, runner)?.Name + isLocal + " \n";
            }
            Debug.Log(players);
            // _lobbyPlayerText.text = players;
            // _lobbyRoomName.text = $"Room: {runner.SessionInfo.Name}";
        }
    }
}