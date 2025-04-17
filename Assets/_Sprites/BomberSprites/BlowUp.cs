using UnityEngine;

public class BlowUp : MonoBehaviour
{
    public float destroyTime = 1f; // Tid før objektet bliver destrueret
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

}
