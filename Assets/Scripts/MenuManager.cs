using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private NetworkRunner _networkRunnerPrefab;
    [SerializeField] private PlayerData _playerDataPrefab;
    [SerializeField] private TMP_InputField _nickNameInputFieldTMP;
    [SerializeField] private TextMeshProUGUI _nickNamePlaceHolder;
    [SerializeField] private TMP_InputField _roomNameInputFieldTMP;
    [SerializeField] private string _gameSceneName;

    private NetworkRunner _runnerInstance = null;

    public void StartHost()
    {
        
    }

    public void StartClient()
    {
        
    }

    private void SetPlayerData()
    {
        var playerData = FindObjectOfType<PlayerData>();
        if (playerData == null)
        {
            playerData = Instantiate(_playerDataPrefab);
        }

        if (string.IsNullOrWhiteSpace(_nickNameInputFieldTMP.text))
        {
            playerData.SetNickName(_nickNamePlaceHolder.text);
        }
        else
        {
            playerData.SetNickName(_nickNameInputFieldTMP.text);
        }
    }

    private async void StartGame(GameMode mode, string roomName, string sceneName)
    {
        _runnerInstance = FindObjectOfType<NetworkRunner>();
        if (_runnerInstance == null)
        {
            _runnerInstance = Instantiate(_networkRunnerPrefab);
        }

        _runnerInstance.ProvideInput = true;
        
        var startGameArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            ObjectPool =  _runnerInstance.GetComponent<>()
        }
    }
}
