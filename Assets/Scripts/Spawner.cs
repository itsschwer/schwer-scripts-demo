using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField] private GameObject prefab = default;
    [SerializeField] private BoxCollider2D spawnBounds = default;
    [SerializeField] private float spawnInterval = default;
    [SerializeField] private int retriesPerSpawn = 100;
    [SerializeField] private int upperLimit = 1000;
    [SerializeField] private int maxLifetimeSpawns = -1;

    private new Transform transform;
    private ContactFilter2D filter;
    private float timer;
    private int spawns;

    private void Awake() {
        transform = GetComponent<Transform>();
        filter = new ContactFilter2D();
        filter.useTriggers = false;
    }

    private void Update() {
        if (transform.childCount <= upperLimit && (spawns <= maxLifetimeSpawns || maxLifetimeSpawns <= 0)) {
            if (timer <= 0) {
                SpawnPrefab();
                timer = spawnInterval;
            }
            else {
                timer -= Time.deltaTime;
            }
        }
    }

    private void SpawnPrefab() {
        var bounds = spawnBounds.bounds;
        for (int i = 0; i < retriesPerSpawn; i++) {
            var point = GetRandomPoint(bounds);
            if (TestPoint(point)) {
                Instantiate(prefab, point, Quaternion.identity, transform);
                spawns++;
                return;
            }
        }
    }

    private Vector2 GetRandomPoint(Bounds bounds) {
        var x = Random.Range(bounds.min.x, bounds.max.x);
        var y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }

    private bool TestPoint(Vector2 point) {
        var collider = prefab.GetComponent<Collider2D>();
        if (collider != null) {
            var array = new Collider2D[1];
            var collisions = Physics2D.OverlapBox(point, collider.bounds.size, 0, filter, array);
            return (collisions <= 0);
        }
        return true;
    }
}
