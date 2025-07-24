using UnityEngine;

public class DoubleJumpEffect : MonoBehaviour, IPowerUpEffect
{
    private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        PowerUpManager.Instance.RegisterPowerUp(PowerUpType.DoubleJump, this);
    }

    public void Activate()
    {
        player.enableDoubleJump = true;
        Debug.Log("Doble salto ACTIVADO");
    }

    public void Deactivate()
    {
        player.enableDoubleJump = false;
        Debug.Log("Doble salto DESACTIVADO");
    }
}

