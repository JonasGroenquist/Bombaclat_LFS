using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.5f;

	private PlayerControls playerControls;
	private Vector2 movement;
	private Rigidbody2D rb;
	private Animator myAnimator;
	private SpriteRenderer spriteRenderer;

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

	public void IncreaseSpeed(float amount)
	{
		moveSpeed += amount;
	}
}
