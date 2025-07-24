using UnityEngine;

public class AttackEffect : MonoBehaviour, IPowerUpEffect
{
    private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        PowerUpManager.Instance.RegisterPowerUp(PowerUpType.Attack, this);
    }

    public void Activate()
    {
        player.enableAttack = true;
        Debug.Log("Ataque ACTIVADO");
    }

    public void Deactivate()
    {
        player.enableAttack = false;
        Debug.Log("Ataque DESACTIVADO");
    }
}
