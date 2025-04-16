using UnityEngine;

public class Enemy : MonoBehaviour
{
    [field: SerializeField]
    private float speed { get; set; } = 2.0f;

    [field: SerializeField]
    private bool move { get; set; } = false;

    [field: SerializeField]
    private Animator animator;


    Vector2 moveDirection = new Vector2();

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Enemy goes in a random direction
        int randomDirection = Random.Range(0, 4);
        switch (randomDirection)
        {
            case 0:
                moveDirection = Vector2.up;
                animator.SetBool("walk", true);
                break;
            case 1:
                moveDirection = Vector2.down;
                animator.SetBool("walk", true);
                break;
            case 2:
                moveDirection = Vector2.left;
                animator.SetBool("walk", true);
                break;
            case 3:
                moveDirection = Vector2.right;
                animator.SetBool("walk", true);
                break;
        }

    }

    void Update()
    {
        if (move)
        {
            rb.MovePosition(rb.position + (moveDirection * speed * Time.deltaTime));
        }

    }
}
