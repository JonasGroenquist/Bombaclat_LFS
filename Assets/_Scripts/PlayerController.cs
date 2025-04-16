using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 1f;

	private PlayerControls playerControls;
	private Vector2 movement;
	private Rigidbody2D rb;
	private Animator myAnimator;

	private void Awake()
	{
		playerControls = new PlayerControls();
		rb = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
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
	}

	private void Move()
	{
		rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
	}
}
