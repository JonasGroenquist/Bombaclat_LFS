using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject blockPrefab;

    void Start()
    {
        float chance = Random.value; // Gives a float between 0.0 and 1.0

        if (chance <= 0.8f) // 80% chance
        {
            Instantiate(blockPrefab, transform.position, Quaternion.identity);
        }
    }
}
