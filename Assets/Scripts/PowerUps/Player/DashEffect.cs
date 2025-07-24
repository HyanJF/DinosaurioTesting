using UnityEngine;

public class DashEffect : MonoBehaviour, IPowerUpEffect
{
    private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        PowerUpManager.Instance.RegisterPowerUp(PowerUpType.Dash, this);
    }

    public void Activate()
    {
        player.enableDash = true;
        Debug.Log("Dash ACTIVADO");
    }

    public void Deactivate()
    {
        player.enableDash = false;
        Debug.Log("Dash DESACTIVADO");
    }
}
