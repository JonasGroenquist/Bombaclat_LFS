using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BombController : MonoBehaviour
{
    public KeyCode inputKey = KeyCode.Space;
    public GameObject bombPrefab;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombsRemaining;

    // Eksplosionen er en prefab, der skal placeres i editoren
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    //public Tilemap wallTilemap;
    //public BlowUp BlowUp;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void Update()
    {
        // Tjekker om spilleren har bomber tilbage og om intputkey er trykket
        if (bombsRemaining > 0 && Input.GetKeyDown(inputKey))
        {
            StartCoroutine(PlaceBomb());
        }
    }

    private IEnumerator PlaceBomb()
    {
        // co-routine: execute logik, pause funktionen, vent, og execute mere logik (Using System.Collections)
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x); // Runder positionen til nærmeste heltal
        position.y = Mathf.Round(position.y); // Runder positionen til nærmeste heltal

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity); // Quarternion.identity = ingen rotation
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime); // venter i bombFuseTime sekunder

        // Opdaterer bombens position til nærmeste heltal for at sikre, at den placeres præcist på gridden
        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);


        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start); // sætter den aktive renderer til start spritet
        explosion.PlayExplosionSound(); // spiller eksplosionslyden
        explosion.DestroyAfter(explosionDuration); // sletter eksplosionsobjektet efter explosionDuration sekunder

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);


        // Slettes senere 
        Destroy(bomb);
        bombsRemaining++;
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        // If length is 0, do nothing
        if (length <= 0) { return; }

        // Move position in the given direction
        position += direction;

        // FIRST CHECK: Look for the tilemap wall
        Tilemap wallTilemap = GameObject.Find("WallForeground").GetComponent<Tilemap>();
        Vector3Int cellPosition = wallTilemap.WorldToCell(position);

        // Check if there's a tile at this position
        if (wallTilemap.HasTile(cellPosition))
        {
            Debug.Log("Wall tile detected at cell: " + cellPosition);
            return; // Stop explosion
        }

        // SECOND CHECK: Look for regular wall GameObjects
        Collider2D[] colliders = Physics2D.OverlapPointAll(position);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Debug.Log("Wall GameObject detected: " + collider.gameObject.name);

                // Get the AnimatedSpriteRenderer component
                AnimatedSpriteRenderer wallRenderer = collider.GetComponent<AnimatedSpriteRenderer>();
                if (wallRenderer != null)
                {
                    // Try to drop a powerup before destruction
                    DestroyWall destroyWall = collider.GetComponent<DestroyWall>();
                    if (destroyWall != null)
                    {
                        // destroyWall.TryDropPowerup();
                    }

                    // Continue with animation
                    if (wallRenderer.GetType().GetMethod("PlayDestructionAnimation") != null)
                    {
                        wallRenderer.PlayDestructionAnimation();
                    }

                    // Destroy after full animation cycle
                    Destroy(collider.gameObject, wallRenderer.animationSprites.Length * wallRenderer.animationTime);
                }
                else
                {
                    // No AnimatedSpriteRenderer, just destroy immediately
                    // Destroy(collider.gameObject);
                }

                return; // Stop explosion
            }
        }


        // No wall detected, continue with explosion
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        if (length > 1)
        { explosion.SetActiveRenderer(explosion.middle); }
        else
        { explosion.SetActiveRenderer(explosion.end); }
        explosion.SetDirection(direction);
        explosion.PlayExplosionSound(); // spiller eksplosionslyden
        explosion.DestroyAfter(explosionDuration);

        // Continue explosion in this direction
        Explode(position, direction, length - 1);
    }


    // Når spilleren bevæger sig væk fra bombens collider, udløses OnTriggerExit2D()
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false; // gør collideren til en solid collider
        }
    }

    //private void ClearBrick(Vector2 position)
    //{
    //    Vector3Int cellPosition = wallTilemap.WorldToCell(position);
    //    TileBase tile = wallTilemap.GetTile(cellPosition);

    //    if (tile != null)
    //    {
    //        Instantiate(BlowUp, position, Quaternion.identity);
    //        wallTilemap.SetTile(cellPosition, null); // Fjern tile
    //    }
    //}

    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }
}
