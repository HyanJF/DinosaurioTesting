using UnityEngine;

[System.Serializable]
public class Genome
{
    public float reactionTime;
    public float jumpThreshold;
    public float lowJumpForce;
    public float midJumpForce;
    public float highJumpForce;
    public float optimalJumpDistance;  // NUEVO gen para distancia óptima

    public Genome(float reactionTime, float jumpThreshold, float low, float mid, float high, float optimalDist)
    {
        this.reactionTime = reactionTime;
        this.jumpThreshold = jumpThreshold;
        this.lowJumpForce = low;
        this.midJumpForce = mid;
        this.highJumpForce = high;
        this.optimalJumpDistance = optimalDist;
    }

    public static Genome RandomGenome()
    {
        return new Genome(
            Random.Range(0.05f, 0.5f),      // reactionTime
            Random.Range(1f, 5f),           // jumpThreshold
            Random.Range(3f, 6f),           // lowJumpForce
            Random.Range(6f, 9f),           // midJumpForce
            Random.Range(9f, 13f),          // highJumpForce
            Random.Range(0.5f, 5f)          // optimalJumpDistance
        );
    }

    public static Genome Crossover(Genome p1, Genome p2)
    {
        return new Genome(
            Random.value < 0.5f ? p1.reactionTime : p2.reactionTime,
            Random.value < 0.5f ? p1.jumpThreshold : p2.jumpThreshold,
            Random.value < 0.5f ? p1.lowJumpForce : p2.lowJumpForce,
            Random.value < 0.5f ? p1.midJumpForce : p2.midJumpForce,
            Random.value < 0.5f ? p1.highJumpForce : p2.highJumpForce,
            Random.value < 0.5f ? p1.optimalJumpDistance : p2.optimalJumpDistance
        );
    }

    public void Mutate(float mutationRate)
    {
        if (Random.value < mutationRate)
            reactionTime += Random.Range(-0.1f, 0.1f);
        if (Random.value < mutationRate)
            jumpThreshold += Random.Range(-0.5f, 0.5f);
        if (Random.value < mutationRate)
            lowJumpForce += Random.Range(-1f, 1f);
        if (Random.value < mutationRate)
            midJumpForce += Random.Range(-1f, 1f);
        if (Random.value < mutationRate)
            highJumpForce += Random.Range(-1f, 1f);
        if (Random.value < mutationRate)
            optimalJumpDistance += Random.Range(-0.5f, 0.5f);

        // Clamping para mantener valores válidos
        reactionTime = Mathf.Clamp(reactionTime, 0.01f, 1f);
        jumpThreshold = Mathf.Clamp(jumpThreshold, 0.5f, 10f);
        lowJumpForce = Mathf.Clamp(lowJumpForce, 2f, 10f);
        midJumpForce = Mathf.Clamp(midJumpForce, 4f, 12f);
        highJumpForce = Mathf.Clamp(highJumpForce, 6f, 15f);
        optimalJumpDistance = Mathf.Clamp(optimalJumpDistance, 0.1f, 10f);
    }
}
