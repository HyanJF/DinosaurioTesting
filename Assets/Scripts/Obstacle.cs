using UnityEngine;

public enum ObstacleType { Low, Medium, High }

public class Obstacle : MonoBehaviour
{
    public float speed = 5f;
    public ObstacleType type;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x < -15f)
            Destroy(gameObject);
    }
}
