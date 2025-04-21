using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.5f;

	private PlayerControls playerControls;
	private Vector2 movement;
	private Rigidbody2D rb;
	private Animator myAnimator;
	private SpriteRenderer spriteRenderer;
	public AnimatedSpriteRenderer spriteRendererDeath;

	private void Awake()
	{
		playerControls = new PlayerControls();
		rb = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnEnable()
	{
		playerControls.Enable();
	}

	private void Update()
	{
		PlayerInput();
	}

	private void FixedUpdate()
	{
		Move();
	}


	private void PlayerInput()
	{
		movement = playerControls.Movement.Move.ReadValue<Vector2>();

		myAnimator.SetFloat("movex", movement.x);
		myAnimator.SetFloat("movey", movement.y);

		// Flip sprite based on horizontal input
		if (movement.x > 0.01f)
		{
			spriteRenderer.flipX = false;
		}
		else if (movement.x < -0.01f)
		{
			spriteRenderer.flipX = true;
		}
	}

	private void Move()
	{
		rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
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
        GetComponent<BombController>().enabled = false;
        spriteRenderer.enabled = false;
        spriteRendererDeath.enabled = true;

        // Force the sprite to animate instead of showing idle
        spriteRendererDeath.idle = false;

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded() 
	{
		gameObject.SetActive(false);
		FindAnyObjectByType<GameManager>().CheckWinState();
    }
}
