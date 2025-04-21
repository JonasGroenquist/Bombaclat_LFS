using UnityEngine;
using UnityEngine.SceneManagement; // Bruger SceneManager til at genstarte scenen

public class GameManager : MonoBehaviour
{
    public GameObject[] players;

    public void CheckWinState()
    {
        int aliveCount = 0;

        foreach (GameObject player in players)
        {
            if (player.activeSelf)
            {
                aliveCount++;
            }
        }
        if (aliveCount <= 1)
        {
            Invoke(nameof(NewRound), 5f);
        }
    }

    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Genstarter den nuværende scene
    }

    // GameManager bliver kaldt i PlayerController, hvor den tjekker CheckWinState()
}
