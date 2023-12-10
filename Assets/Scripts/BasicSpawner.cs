using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct NetworkInputData : INetworkInput
{
    public Vector3 move;
    public NetworkBool aiming;
    public NetworkBool sliding;
    public NetworkBool shoot;
    public NetworkBool sprint;
}

public class BasicSpawner : NetworkBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private async void StartGame(GameMode mode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        
        await _runner.StartGame(new StartGameArgs
        {
            GameMode = mode,
            SessionName = "TestRoom", // 세션 이름(클라이언트와 서버 세션 이름)
            Scene = SceneManager.GetActiveScene().buildIndex, // Scene은 구조체인 SceneRef?타입
            // NetworkSceneManagerDefault : 비동기 씬 관련 메서드가 포함된 클래스
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(), // SceneManager INetworkSceneManager 타입이다.
        });
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0,0,200,40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0,40,200,40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 0, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            _spawnedCharacters.Add(player, networkPlayerObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData data = new NetworkInputData();
        if (Input.GetKey(KeyCode.W))
            data.move += Vector3.up;

        if (Input.GetKey(KeyCode.S))
            data.move += Vector3.down;

        if (Input.GetKey(KeyCode.A))
            data.move += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.move += Vector3.right;
        data.move = data.move.normalized;
        data.sprint = Input.GetKey(KeyCode.LeftShift);
        data.aiming = Input.GetMouseButton(1);
        
        input.Set(data);
    }

    private void PrintData(NetworkInputData data)
    {
        Debug.Log($"{data.move}\n {data.aiming}");
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
}
