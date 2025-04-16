using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum AIState { Idle, Walking, SeekingCover }
    private AIState currentState = AIState.Idle;

    [field: SerializeField] private float speed { get; set; } = 2.0f;
    [field: SerializeField] private bool move { get; set; } = false;
    [field: SerializeField] private Animator animator;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float checkDistance = 0.7f;
    [SerializeField] private Vector2 boxSize = new Vector2(0.4f, 0.4f);
    [SerializeField] private float idleTimeMin = 1f;
    [SerializeField] private float idleTimeMax = 2.5f;

    private Vector2 moveDirection = Vector2.zero;
    private Rigidbody2D rb;
    private float changeDirectionCooldown = 0.5f;
    private float cooldownTimer = 0f;
    private float stateTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Debug.Log($"{name}: Startet – går i Idle.");
        EnterIdleState();
    }

    void FixedUpdate()
    {
        cooldownTimer -= Time.fixedDeltaTime;
        stateTimer -= Time.fixedDeltaTime;

        switch (currentState)
        {
            case AIState.Idle:
                if (stateTimer <= 0f)
                {
                    Debug.Log($"{name}: Færdig med Idle – går i Walk.");
                    EnterWalkState();
                }
                break;

            case AIState.Walking:
                HandleMovement();
                break;

            case AIState.SeekingCover:
                HandleMovement();
                if (stateTimer <= 0f)
                {
                    Debug.Log($"{name}: Har søgt dækning – går i Idle.");
                    EnterIdleState();
                }
                break;
        }
    }

    void HandleMovement()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0f, moveDirection, checkDistance);

        if (hit.collider != null && hit.collider != rb.GetComponent<Collider2D>() && cooldownTimer <= 0f)
        {
            Debug.Log($"{name}: Rammer noget med tag '{hit.collider.tag}'.");

            if (hit.collider.CompareTag("Indestructible"))
            {
                Debug.Log($"{name}: Ramte en indestructible væg – skifter retning.");
                ChooseNewDirection();
                cooldownTimer = changeDirectionCooldown;
                return;
            }
            else if (hit.collider.CompareTag("Wall"))
            {
                Debug.Log($"{name}: Ramte en almindelig væg – placerer bombe og søger dækning.");
                PlaceBomb();
                EnterSeekCoverState();
                cooldownTimer = changeDirectionCooldown;
                return;
            }
        }

        rb.MovePosition(rb.position + (moveDirection * speed * Time.fixedDeltaTime));
    }


    void ChooseNewDirection()
    {
        int randomDirection = Random.Range(0, 4);
        switch (randomDirection)
        {
            case 0: moveDirection = Vector2.up; Debug.Log($"{name}: Vælger ny retning: OP"); break;
            case 1: moveDirection = Vector2.down; Debug.Log($"{name}: Vælger ny retning: NED"); break;
            case 2: moveDirection = Vector2.left; Debug.Log($"{name}: Vælger ny retning: VENSTRE"); break;
            case 3: moveDirection = Vector2.right; Debug.Log($"{name}: Vælger ny retning: HØJRE"); break;
        }

        animator.SetBool("walk", true);
    }

    void EnterIdleState()
    {
        currentState = AIState.Idle;
        animator.SetBool("walk", false);
        move = false;
        stateTimer = Random.Range(idleTimeMin, idleTimeMax);
        Debug.Log($"{name}: Går i Idle i {stateTimer:F2} sekunder.");
    }

    void EnterWalkState()
    {
        currentState = AIState.Walking;
        ChooseNewDirection();
        animator.SetBool("walk", true);
        move = true;
        stateTimer = Random.Range(2f, 5f);
        Debug.Log($"{name}: Går i Walk i {stateTimer:F2} sekunder.");
    }

    void EnterSeekCoverState()
    {
        currentState = AIState.SeekingCover;
        moveDirection = -moveDirection;
        animator.SetBool("walk", true);
        move = true;
        stateTimer = Random.Range(1.5f, 3f);
        Debug.Log($"{name}: Søger dækning i {stateTimer:F2} sekunder.");
    }

    void PlaceBomb()
    {
        if (bombPrefab != null)
        {
            Instantiate(bombPrefab, transform.position, Quaternion.identity);
            Debug.Log($"{name}: Bombe placeret på {transform.position}");
        }
    }
}
