using Quantum;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public unsafe class UIScore : MonoBehaviour {
    [FormerlySerializedAs("textPrefab")]
    [SerializeField]
    private TextMeshProUGUI _textPrefab;

    private TextMeshProUGUI _scoreText;

    private PlayerLink* _playerLink;

    public int PlayerIndex => _playerLink->Player._index;

    public int Score => _playerLink->score;

    private void Start() {
        EntityViewUpdater viewUpdater = FindFirstObjectByType<EntityViewUpdater>(FindObjectsInactive.Include);
        Frame verifiedFrame = viewUpdater.ObservedGame.Frames.Verified;
        EntityRef entityRef = GetComponent<EntityView>().EntityRef;
        _playerLink = verifiedFrame.Unsafe.GetPointer<PlayerLink>(entityRef);
        if (_playerLink->Equals(null))
            return;
        Transform parent = GameObject.Find("Score Container").transform;
        _scoreText = Instantiate(_textPrefab, parent);
        _scoreText.text = "Player " + PlayerIndex + "\nScore: " + Score;
    }

    private void LateUpdate() {
        if (_scoreText != null)
            _scoreText.text = "Player " + PlayerIndex + "\nScore: " + Score;
    }
}