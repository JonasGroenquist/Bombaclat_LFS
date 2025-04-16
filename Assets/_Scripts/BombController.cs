using System.Collections;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public KeyCode inputKey = KeyCode.Space;
    public GameObject bombPrefab;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombsRemaining;

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
        position.x = Mathf.Round(position.x); // Runder positionen til n�rmeste heltal
        position.y = Mathf.Round(position.y); // Runder positionen til n�rmeste heltal

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity); // Quarternion.identity = ingen rotation
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime); // venter i bombFuseTime sekunder

        // Slettes senere 
        Destroy(bomb);
        bombsRemaining++;
    }

    // N�r spilleren bev�ger sig v�k fra bombens collider, udl�ses OnTriggerExit2D()
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false; // g�r collideren til en solid collider
        }
    }



}
