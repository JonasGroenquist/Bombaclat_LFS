using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    [Header("Powerup Settings")]
    public GameObject[] powerupPrefabs;
    public float dropChance = 0.3f;

    // Add this new method that will be called explicitly
    public void TryDropPowerup()
    {
        // Check if this wall should drop a powerup
        if (Random.value < dropChance && powerupPrefabs.Length > 0)
        {
            // Randomly select a powerup from the array
            int randomIndex = Random.Range(0, powerupPrefabs.Length);

            // Spawn the selected powerup at the wall's position
            Instantiate(powerupPrefabs[randomIndex], transform.position, Quaternion.identity);
            Debug.Log("Powerup dropped!");
        }
    }

    // Keep OnDestroy as a backup
    private void OnDestroy()
    {
        // Don't drop items when scene is changing or application is quitting
        if (!gameObject.scene.isLoaded) return;

        // Try to drop powerup
        TryDropPowerup();
    }
}