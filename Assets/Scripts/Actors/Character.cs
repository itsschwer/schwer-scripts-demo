using UnityEngine;

public abstract class Character : Actor {
    [SerializeField] private float speed = 5;

    protected virtual void Update() {
        GetInput();

        if (state == State.Free) {
            SetAnimationXY(input.direction);
            animator.SetBool("Moving", (input.direction != Vector2.zero));
        }
    }

    protected abstract void GetInput();

    protected virtual void FixedUpdate() {
        if (state == State.Free) {
            rigidbody.MovePosition(transform.position + (Vector3)(input.direction.normalized * speed * Time.deltaTime));
        }
    }
}
