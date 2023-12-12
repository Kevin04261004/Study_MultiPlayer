using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    [SerializeField] private TMP_InputField _chatInputFieldTMP;

    private void Awake()
    {
        _chatInputFieldTMP.onSubmit.AddListener(delegate { OnSubmit();});
    }

    private void OnSubmit()
    {
        if (string.IsNullOrEmpty(_chatInputFieldTMP.text))
        {
            return;
        }
        RPC_SendMessage(_chatInputFieldTMP.text);
        _chatInputFieldTMP.text = "";
        _chatInputFieldTMP.ActivateInputField();
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        RPC_RelayMessage(message, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayMessage(string message, PlayerRef messageSource)
    {
        if (messageSource == Runner.LocalPlayer)
        {
            message = $"You said: {message}\n";
        }
        else
        {
            message = $"Some other player said: {message}\n";
        }
        Debug.Log(message);
    }
}
