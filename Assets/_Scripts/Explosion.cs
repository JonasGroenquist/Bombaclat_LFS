using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AnimatedSpriteRenderer start;
    public AnimatedSpriteRenderer middle;
    public AnimatedSpriteRenderer end;

    public void SetActiveRenderer(AnimatedSpriteRenderer renderer)
    {
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    public void SetDirection(Vector2 direction)
    {
        // Regner viklen ud fra retningen og sætter rotationen
        float angle = Mathf.Atan2(direction.y, direction.x);
        // Mathf.Rad2Deg bruges til at konvertere vinklen fra radianer (som Mathf.Atan2 returnerer) til grader,
        // da Unity's rotationer (Quaternion.AngleAxis) arbejder med grader i stedet for radianer.
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }
}
