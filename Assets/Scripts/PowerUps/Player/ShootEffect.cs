using UnityEngine;

public class ShootEffect : MonoBehaviour, IPowerUpEffect
{
    private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        PowerUpManager.Instance.RegisterPowerUp(PowerUpType.Shoot, this);
    }

    public void Activate()
    {
        player.enableShoot = true;
        Debug.Log("Disparo ACTIVADO");
    }

    public void Deactivate()
    {
        player.enableShoot = false;
        Debug.Log("Disparo DESACTIVADO");
    }
}
