using Quantum;

public class QuantumCustomCallbacks : QuantumCallbacks {
    public override void OnGameStart(QuantumGame game) {
        foreach (int lp in game.GetLocalPlayers()) game.SendPlayerData(lp, new RuntimePlayer { });
    }
}