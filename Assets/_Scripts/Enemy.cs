using UnityEngine;

public class Enemy : MonoBehaviour
{
    [field: SerializeField]
    private float speed { get; set; } = 2.0f;

    [field: SerializeField]
    private bool move { get; set; } = false;

    [field: SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject bombPrefab; // Prefab til bomben

    [SerializeField]
    private float checkDistance = 0.7f; // Hvor langt den tjekker fremad

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
            // Tjek hvad der er foran fjenden
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, checkDistance);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Indestructible"))
                {
                    ChooseNewDirection();
                }
                else if (hit.collider.CompareTag("Wall"))
                {
                    PlaceBomb();
                    ChooseNewDirection(); // Hvis ønsket
                }
            }

            rb.MovePosition(rb.position + (moveDirection * speed * Time.deltaTime));
        }
    }

    void ChooseNewDirection()
    {
        // Vælg en ny tilfældig retning
        int randomDirection = Random.Range(0, 4);
        switch (randomDirection)
        {
            case 0:
                moveDirection = Vector2.up;
                break;
            case 1:
                moveDirection = Vector2.down;
                break;
            case 2:
                moveDirection = Vector2.left;
                break;
            case 3:
                moveDirection = Vector2.right;
                break;
        }
    }

    void PlaceBomb()
    {
        if (bombPrefab != null)
        {
            Instantiate(bombPrefab, transform.position, Quaternion.identity);
        }
    }
}
