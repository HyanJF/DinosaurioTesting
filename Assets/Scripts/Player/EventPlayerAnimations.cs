using UnityEngine;

public class EventPlayerAnimations : MonoBehaviour
{
    public PlayerController pC;
    public AttackHitbox aH;

    public void DeathPlayer()
    {
        pC.GameOver();
    }

    public void AttackActivate()
    {
        aH.Activate();
    }

    public void AttackDesactivate()
    {
        aH.Deactivate();
    }
}
