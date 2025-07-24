using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    private Dictionary<PowerUpType, IPowerUpEffect> activeEffects = new();
    private Dictionary<PowerUpType, Coroutine> timers = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterPowerUp(PowerUpType type, IPowerUpEffect effect)
    {
        if (!activeEffects.ContainsKey(type))
        {
            activeEffects[type] = effect;
        }
    }

    public void ActivatePowerUp(PowerUpType type, float duration = 5f)
    {
        if (activeEffects.TryGetValue(type, out IPowerUpEffect effect))
        {
            effect.Activate();

            if (timers.ContainsKey(type))
                StopCoroutine(timers[type]);

            timers[type] = StartCoroutine(DeactivateAfterTime(type, duration));
        }
        else
        {
            Debug.LogWarning($"PowerUp '{type}' no está registrado.");
        }
    }

    private IEnumerator<WaitForSeconds> DeactivateAfterTime(PowerUpType type, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (activeEffects.TryGetValue(type, out IPowerUpEffect effect))
        {
            effect.Deactivate();
            timers.Remove(type);
        }
    }
}
