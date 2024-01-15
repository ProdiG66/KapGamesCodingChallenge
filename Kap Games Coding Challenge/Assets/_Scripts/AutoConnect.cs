using System;
using ExitGames.Client.Photon;
using Quantum;
using UnityEngine;
using Random = UnityEngine.Random;


public class AutoConnect : ConnectionCallbacks {
    [SerializeField]
    private AssetGuid selectedMapGuid;

    protected virtual void Update() {
        LoadBalancingClient?.Service();

        if (LoadBalancingClient != null && LoadBalancingClient.InRoom) {
            bool hasStarted = LoadBalancingClient.CurrentRoom.CustomProperties.TryGetValue("START", out object start) && (bool)start;
            AssetGuid mapGuid = (AssetGuid)(LoadBalancingClient.CurrentRoom.CustomProperties.TryGetValue("MAP-GUID", out object guid) ? (long)guid : 0L);

            if (LoadBalancingClient.LocalPlayer.IsMasterClient) {
                Hashtable ht = new();
                if (!mapGuid.IsValid)
                    if (selectedMapGuid.IsValid)
                        ht.Add("MAP-GUID", selectedMapGuid.Value);
                if (!hasStarted)
                    ht.Add("START", true);
                if (ht.Count > 0) LoadBalancingClient.CurrentRoom.SetCustomProperties(ht);
            }

            if (mapGuid.IsValid && hasStarted) {
                Debug.LogFormat("### Starting game using map '{0}'", mapGuid);

                RuntimeConfig config = Config != null ? RuntimeConfig.FromByteArray(RuntimeConfig.ToByteArray(Config)) : new RuntimeConfig();
                config.Map.Id = mapGuid;
                config.Seed = Random.Range(int.MinValue, int.MaxValue);

                QuantumRunner.StartParameters param = new() {
                    RuntimeConfig = config,
                    DeterministicConfig = DeterministicSessionConfigAsset.Instance.Config,
                    GameMode = Photon.Deterministic.DeterministicGameMode.Multiplayer,
                    PlayerCount = LoadBalancingClient.CurrentRoom.MaxPlayers,
                    LocalPlayerCount = 1,
                    NetworkClient = LoadBalancingClient
                };

                string clientId = Guid.NewGuid().ToString();
                QuantumRunner.StartGame(clientId, param);

                mainMenuObj.SetActive(false);
            }
        }
    }
}