using UnityEngine;

public class ShieldEffect : MonoBehaviour, IPowerUpEffect
{
    private PlayerController player;
    public ParticleSystem shieldParticles;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        PowerUpManager.Instance.RegisterPowerUp(PowerUpType.Shield, this);

        if (shieldParticles == null)
            Debug.LogWarning("No se asignó el ParticleSystem de escudo.");
        else
            shieldParticles.Stop();  // Asegurarse que no esté activo al inicio
    }

    public void Activate()
    {
        player.enableShield = true;
        Debug.Log("Escudo ACTIVADO");

        if (shieldParticles != null)
            shieldParticles.Play();
    }

    public void Deactivate()
    {
        player.enableShield = false;
        Debug.Log("Escudo DESACTIVADO");

        if (shieldParticles != null)
            shieldParticles.Stop();
    }
}
