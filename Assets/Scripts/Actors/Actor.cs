using UnityEngine;

public abstract class Actor : MonoBehaviour {
    public bool frozen { get; set; }
    public new Transform transform { get; private set; }
    public new Rigidbody2D rigidbody { get; private set; }
    protected Animator animator;

    protected ActorInput input;
    public Vector2 facingDirection => input.directionNonZero;

    protected virtual void Awake() {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected void SetAnimationXY(Vector2 direction) {
        direction.Normalize();
        if (direction != Vector2.zero) {
            direction.x = Mathf.Round(direction.x);
            direction.y = Mathf.Round(direction.y);

            animator.SetFloat("X", direction.x);
            animator.SetFloat("Y", direction.y);
        }
    }
}
