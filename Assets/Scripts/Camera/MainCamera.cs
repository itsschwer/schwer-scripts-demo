using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviourSingleton<MainCamera> {
    public new Camera camera { get; private set; }
    public new Transform transform { get; private set; }

    [SerializeField] private Transform target = default;
    [SerializeField] private Vector2 offset = default;
    [SerializeField] private BoxCollider2D boundingBox = default;

    protected override void Awake() {
        base.Awake();
        transform = GetComponent<Transform>();
        camera = GetComponent<Camera>();
    }

    private void FixedUpdate() {
        var position = BoundPosition(target.position + (Vector3)offset);
        position.z = transform.position.z;
        transform.position = position;
    }

    private Vector3 BoundPosition(Vector3 position) {
        if (boundingBox != null) {
            var y = camera.orthographicSize;
            var x = y * camera.aspect;
            var bounds = boundingBox.bounds;
            position.x = Mathf.Clamp(position.x, bounds.min.x + x, bounds.max.x - x);
            position.y = Mathf.Clamp(position.y, bounds.min.y + y, bounds.max.y - y);
        }
        return position;
    }
}
