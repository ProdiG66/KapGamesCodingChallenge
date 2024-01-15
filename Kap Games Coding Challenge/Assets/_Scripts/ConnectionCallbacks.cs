using System.Collections.Generic;
using Photon.Realtime;
using Quantum;
using UnityEngine;
using UnityEngine.Serialization;


public class ConnectionCallbacks : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks {
    public byte maxPlayers = 4;

    [FormerlySerializedAs("MainMenuObj")]
    [SerializeField]
    protected GameObject mainMenuObj;

    protected QuantumLoadBalancingClient LoadBalancingClient;
    protected RuntimeConfig Config;

    public void OnDestroy() {
        if (LoadBalancingClient != null && LoadBalancingClient.IsConnected == true) LoadBalancingClient.Disconnect();
    }

    #region ConnectionCallbacks

    public void OnConnected() { }

    public void OnConnectedToMaster() {
        LoadBalancingClient.OpJoinRandomRoom(new OpJoinRandomRoomParams { MatchingType = MatchmakingMode.FillRoom });
    }

    public void OnDisconnected(DisconnectCause cause) {
        Debug.Log($"Disconnected: {cause}");
        QuantumRunner.ShutdownAll(true);
        mainMenuObj.SetActive(true);
    }

    public void OnRegionListReceived(RegionHandler regionHandler) { }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }

    public void OnCustomAuthenticationFailed(string debugMessage) { }

    #endregion

    #region MatchmakingCallbacks

    public void OnFriendListUpdate(List<FriendInfo> friendList) { }

    public void OnCreatedRoom() { }

    public void OnCreateRoomFailed(short returnCode, string message) { }

    public void OnJoinedRoom() {
        Debug.LogFormat("Connected to room '{0}' and waiting for other players (isMasterClient = {1})",
            LoadBalancingClient.CurrentRoom.Name,
            LoadBalancingClient.LocalPlayer.IsMasterClient);
    }

    public void OnJoinRoomFailed(short returnCode, string message) { }

    public void OnJoinRandomFailed(short returnCode, string message) {
        RoomOptions roomOptions = new() {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = maxPlayers,
            Plugins = new string[] { "QuantumPlugin" }
        };

        LoadBalancingClient.OpCreateRoom(new EnterRoomParams() { RoomOptions = roomOptions });

        Debug.LogWarningFormat("Creating new room for '{0}' max players", maxPlayers);
    }

    public void OnLeftRoom() { }

    #endregion
}