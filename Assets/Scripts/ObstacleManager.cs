using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Asegúrate de importar esto

public class ObstacleManager : MonoBehaviour
{
    [Header("Obstacles")]
    public GameObject lowObstaclePrefab;
    public GameObject midObstaclePrefab;
    public GameObject highObstaclePrefab;

    public Transform spawnPoint;

    [Header("Tiempos de Spawn")]
    public float initialSpawnInterval = 8f;
    public float minSpawnInterval = 1.5f;
    public float decreaseRate = 0.5f;
    public float timeBetweenReductions = 1f;

    [Header("Velocidad")]
    private float currentSpawnInterval;
    private float obstacleSpeed = 5f;
    private List<GameObject> spawnedObstacles = new List<GameObject>();

    [Header("Rondas")]
    public int maxRounds = 5;
    private int currentRound = 1;

    public GeneticAlgorithmManager geneticManager; // Referencia al manager para acceder a los agentes

    public TextMeshProUGUI roundText;  // Referencia UI para mostrar la ronda

    private bool simulationFinished = false;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(SpawnObstacleRoutine());
        StartCoroutine(AdjustDifficultyRoutine());
        StartCoroutine(FadeRoundTextRoutine(currentRound)); // Mostrar ronda 1 con fade
    }

    IEnumerator SpawnObstacleRoutine()
    {
        while (!simulationFinished)
        {
            SpawnObstacle();
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    IEnumerator AdjustDifficultyRoutine()
    {
        while (!simulationFinished)
        {
            yield return new WaitForSeconds(timeBetweenReductions);

            if (currentSpawnInterval > minSpawnInterval)
            {
                currentSpawnInterval -= decreaseRate;
                currentSpawnInterval = Mathf.Max(currentSpawnInterval, minSpawnInterval);
            }
            else
            {
                // Termina la ronda actual, aumenta dificultad y ronda
                currentSpawnInterval = initialSpawnInterval;
                obstacleSpeed += 1f;

                // Incrementar ronda
                currentRound++;

                // Mostrar texto de ronda con fade
                StartCoroutine(FadeRoundTextRoutine(currentRound));

                // Aumentar progreso de cada agente vivo
                if (geneticManager != null)
                {
                    foreach (var agent in geneticManager.GetAgents())
                    {
                        if (agent != null && agent.isAlive)
                        {
                            agent.reachedGoalRound = currentRound;
                        }
                    }
                }

                Debug.Log($"⚠️ Reinicio de intervalo. Nueva velocidad de obstáculos: {obstacleSpeed}");

                // Revisar si se alcanzó la ronda máxima para finalizar simulación
                if (currentRound > maxRounds)
                {
                    simulationFinished = true;
                    Debug.Log("✅ Ronda máxima alcanzada. Simulación finalizada.");
                    if (geneticManager != null)
                    {
                        geneticManager.FinishSimulation();
                    }
                }
            }
        }
    }

    void SpawnObstacle()
    {
        int rand = Random.Range(0, 3);
        GameObject prefabToSpawn = rand switch
        {
            0 => lowObstaclePrefab,
            1 => midObstaclePrefab,
            _ => highObstaclePrefab
        };

        GameObject newObstacle = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

        Obstacle obsScript = newObstacle.GetComponent<Obstacle>();
        if (obsScript != null)
            obsScript.speed = obstacleSpeed;

        spawnedObstacles.Add(newObstacle);
        Destroy(newObstacle, 20f);
    }

    public void ClearObstacles()
    {
        foreach (GameObject obj in spawnedObstacles)
        {
            if (obj != null)
                Destroy(obj);
        }
        spawnedObstacles.Clear();
    }

    private IEnumerator FadeRoundTextRoutine(int round)
    {
        if (roundText == null)
            yield break;

        roundText.text = $"Ronda {round}";

        // Fade In
        for (float alpha = 0; alpha <= 1f; alpha += Time.deltaTime * 2f)
        {
            SetTextAlpha(alpha);
            yield return null;
        }
        SetTextAlpha(1f);

        // Tiempo visible
        yield return new WaitForSeconds(1.5f);

        // Fade Out
        for (float alpha = 1f; alpha >= 0; alpha -= Time.deltaTime * 2f)
        {
            SetTextAlpha(alpha);
            yield return null;
        }
        SetTextAlpha(0f);
    }

    private void SetTextAlpha(float alpha)
    {
        Color c = roundText.color;
        c.a = alpha;
        roundText.color = c;
    }
}
