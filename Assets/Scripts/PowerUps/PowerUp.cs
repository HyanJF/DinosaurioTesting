using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public float duration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PowerUpManager.Instance.ActivatePowerUp(type, duration);
            Destroy(gameObject);
        }
    }
}

