using UnityEngine;

public abstract class Character : Actor {
    [SerializeField] private float speed = 5;

    protected virtual void Update() {
        if (frozen) return;

        GetInput();

        SetAnimationXY(input.directionNonZero);
        animator.SetBool("Moving", (input.direction != Vector2.zero));
    }

    protected abstract void GetInput();

    protected virtual void FixedUpdate() => rigidbody.MovePosition(transform.position + (Vector3)(input.direction.normalized * speed * Time.deltaTime));
}
