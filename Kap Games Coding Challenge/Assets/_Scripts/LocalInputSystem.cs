using UnityEngine;
using Photon.Deterministic;
using Quantum;

public class LocalInputSystem : MonoBehaviour {
    private MasterInput _masterInput;

    private void Awake() {
        _masterInput = new MasterInput();
    }

    private void OnEnable() {
        _masterInput.Gameplay.Enable();
        QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
    }

    private void OnDisable() {
        _masterInput.Gameplay.Disable();
    }

    public void PollInput(CallbackPollInput callback) {
        Quantum.Input i = new() {
            Deflect = _masterInput.Gameplay.Deflect.IsPressed(),
            Direction = _masterInput.Gameplay.Move.ReadValue<Vector2>().ToFPVector2()
        };

        callback.SetInput(i, DeterministicInputFlags.Repeatable);
    }
}