using Quantum;
using UnityEngine;

public unsafe class PlayerComponent : MonoBehaviour {
    private PlayerLink* _playerLink;

    private int PlayerIndex => _playerLink->Player._index;

    private int DeflectCount => _playerLink->deflectCount;

    [SerializeField]
    private Renderer sphere;

    [SerializeField]
    private GameObject deflectShield;

    private void Start() {
        EntityViewUpdater viewUpdater = FindFirstObjectByType<EntityViewUpdater>(FindObjectsInactive.Include);
        Frame verifiedFrame = viewUpdater.ObservedGame.Frames.Verified;
        EntityRef entityRef = GetComponent<EntityView>().EntityRef;
        _playerLink = verifiedFrame.Unsafe.GetPointer<PlayerLink>(entityRef);


        if (_playerLink->Equals(null))
            return;

        Color playerColor = Color.white;

        switch (PlayerIndex) {
            case 1:
                playerColor = Color.red;
                break;
            case 2:
                playerColor = Color.blue;
                break;
            case 3:
                playerColor = Color.green;
                break;
            case 4:
                playerColor = new Color(1, 0.6f, 0);
                break;
        }

        sphere.material.SetColor("_Color", playerColor);
    }

    private void Update() {
        if (_playerLink->Equals(null))
            return;

        deflectShield.SetActive(DeflectCount < 1);
    }
}