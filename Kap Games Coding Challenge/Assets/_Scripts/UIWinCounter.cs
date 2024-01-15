using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIWinCounter : MonoBehaviour {
    private List<UIScore> _scores;
    private TextMeshProUGUI _winCount;

    private int Wins {
        get => PlayerPrefs.GetInt("Wins", 0);
        set => PlayerPrefs.SetInt("Wins", value);
    }

    private void Awake() {
        GameObject winCountObj = GameObject.Find("WinCountText");
        if (winCountObj != null)
            _winCount = winCountObj.GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate() {
        if (_winCount != null)
            _winCount.text = "Wins: " + Wins;
        DisconnectOnWin();
    }

    private void DisconnectOnWin() {
        _scores = FindObjectsByType<UIScore>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        if (_scores.Count <= 0)
            return;
        UIScore winner = _scores.Find(element => element.Score >= 3);
        if (winner != null) {
            if (QuantumRunner.Default.Game.GetLocalPlayers().ToList().Contains(winner.PlayerIndex - 1)) {
                Debug.LogWarning("Winner! " + QuantumRunner.Default.Game.GetLocalPlayers().ToList().Find(element => element == winner.PlayerIndex));
                Wins++;
            }

            if (UIAutoConnect.Instance != null)
                UIAutoConnect.Instance.DisconnectGame();
        }
    }
}