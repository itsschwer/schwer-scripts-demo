using UnityEngine;

[System.Serializable]
public struct ActorInput {
    private Vector2 _direction;
    public Vector2 direction {
        get => _direction;
        set {
            _direction = value;
            if (_direction != Vector2.zero) {
                directionNonZero = _direction;
            }
        }
    }
    public Vector2 directionNonZero { get; private set; }

    public bool attack;
}
