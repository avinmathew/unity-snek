using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D GameArea;
    
    private Snake _snake1;
    private Snake2 _snake2;

    void Start()
    {
        _snake1 = FindObjectOfType<Snake>();
        _snake2 = FindObjectOfType<Snake2>();
        RandomisePosition();
    }

    public void RandomisePosition()
    {
        // Keep randomising position until it's in an empty area
        float x, y;
        do
        {
            Bounds bounds = GameArea.bounds;
            x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
            y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));
            transform.position = new Vector3(x, y, 0);
        } while (_snake1.Occupies(x, y) || (_snake2 != null && _snake2.Occupies(x, y)));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RandomisePosition();
    }
}
