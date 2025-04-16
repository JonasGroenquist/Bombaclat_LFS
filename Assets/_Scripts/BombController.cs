using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public KeyCode inputKey = KeyCode.Space;
    public GameObject bombPrefab;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombsRemaining;

    // Eksplosionen er en prefab, der skal placeres i editoren
    public Explosion explosionPrefab;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

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
        // hvis længden er 0, gør ikke noget
        if (length <= 0) { return; }

        position += direction; // flytter positionen i den givne retning

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        if (length > 1)
        { explosion.SetActiveRenderer(explosion.middle); }
        else
        { explosion.SetActiveRenderer(explosion.end); }
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

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



}
