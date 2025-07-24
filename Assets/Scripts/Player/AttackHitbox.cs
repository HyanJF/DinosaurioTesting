using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public string targetTag = "Obstacle";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))  
        {
            Destroy(other.gameObject); // Aquí podrías aplicar daño en lugar de destruir
        }
    }

    public void Activate() => gameObject.SetActive(true);
    public void Deactivate() => gameObject.SetActive(false);
}
