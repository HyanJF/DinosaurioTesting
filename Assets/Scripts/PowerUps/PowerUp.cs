using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public float duration = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PowerUpManager.Instance.ActivatePowerUp(type, duration);
            Destroy(gameObject);
        }
    }
}

