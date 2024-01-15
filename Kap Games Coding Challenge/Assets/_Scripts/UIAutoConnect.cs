using TMPro;
using UnityEngine;


public class UIAutoConnect : AutoConnect {
    public static UIAutoConnect Instance;

    private void Awake() {
        if (Instance == null)
            Instance = this;
    }

    [SerializeField]
    private TextMeshProUGUI winsText;

    [SerializeField]
    private TextMeshProUGUI connectingText;

    public void PlayGame() {
        PhotonServerSettings serverSettings = PhotonServerSettings.Instance;

        if (string.IsNullOrEmpty(serverSettings.AppSettings.AppIdRealtime)) Debug.LogError("AppId not set");

        LoadBalancingClient = new QuantumLoadBalancingClient();
        LoadBalancingClient.ConnectionCallbackTargets.Add(this);
        LoadBalancingClient.MatchMakingCallbackTargets.Add(this);
        LoadBalancingClient.AppId = serverSettings.AppSettings.AppIdRealtime;
        LoadBalancingClient.AppVersion = serverSettings.AppSettings.AppVersion;
        LoadBalancingClient.ConnectToRegionMaster(serverSettings.AppSettings.FixedRegion);
        connectingText.gameObject.SetActive(true);
    }

    public void DisconnectGame() {
        LoadBalancingClient.Disconnect();
        connectingText.gameObject.SetActive(false);
    }

    protected override void Update() {
        winsText.text = "Wins: " + PlayerPrefs.GetInt("Wins", 0);
        base.Update();
    }
}