using UnityEngine;

public class BouncingObject : MonoBehaviour
{
    public Vector2 velocity = new Vector2(3f, 2f);
    public float minX = -8f, maxX = 8f, minY = -4f, maxY = 4f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = velocity;
    }

    void FixedUpdate()
    {
        Vector2 pos = rb.position;

        // Rebotar en X
        if ((pos.x <= minX && velocity.x < 0) || (pos.x >= maxX && velocity.x > 0))
        {
            velocity.x *= -1;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
        }

        // Rebotar en Y
        if ((pos.y <= minY && velocity.y < 0) || (pos.y >= maxY && velocity.y > 0))
        {
            velocity.y *= -1;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
        }

        // Mover usando física
        rb.MovePosition(pos + velocity * Time.fixedDeltaTime);
    }
}
