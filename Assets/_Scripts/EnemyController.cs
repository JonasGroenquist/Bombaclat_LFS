using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    public AnimatedSpriteRenderer spriteRendererDeath;

    private float changeDirectionTime = 0f;
    private float directionChangeInterval = 1f; // seconds

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Animate();
        changeDirectionTime -= Time.deltaTime;

        if (changeDirectionTime <= 0f)
        {
            DecideMovement();
            changeDirectionTime = directionChangeInterval;
        }

        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void DecideMovement()
    {
        // Placeholder AI: gå tilfældigt rundt
        movement = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
    }
    // Called when hitting a wall or obstacle
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Optional: check if it's a wall by tag/layer
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Try a new direction immediately
            DecideMovement();
            changeDirectionTime = directionChangeInterval; // reset timer
        }
        if (collision.gameObject.CompareTag("Indestructible"))
        {
            // Try a new direction immediately
            DecideMovement();
            changeDirectionTime = directionChangeInterval; // reset timer
        }
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void Animate()
    {
        myAnimator.SetFloat("movex", movement.x);
        myAnimator.SetFloat("movey", movement.y);
        // myAnimator.SetBool("walk", true);

        if (movement.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (movement.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Explosion"))
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        spriteRendererDeath.enabled = true;
        spriteRendererDeath.idle = false;

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        FindAnyObjectByType<GameManager>().CheckWinState();
    }
}
