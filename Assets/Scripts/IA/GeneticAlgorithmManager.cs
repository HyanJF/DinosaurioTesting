using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GeneticAlgorithmManager : MonoBehaviour
{
    [Header("Configuración")]
    public int populationSize = 10;
    public float mutationRate = 0.1f;
    public int parentsToSelect = 4;

    [Header("Prefabs y referencias")]
    public GameObject dinoPrefab;
    public Transform spawnPoint;
    public Text populationText;
    public Text parentsText;
    public Slider mutationSlider;

    private List<DinoAgent> population = new List<DinoAgent>();
    private int generation = 0;
    public ObstacleManager obstacleManager;
    public BestAgentsui bestAgentsUI;
    public SimulationEndUI simulationEndUI;
    private bool simulationFinished = false;

    void Start()
    {
        mutationSlider.onValueChanged.AddListener(SetMutationRateFromSlider);
        CreateInitialPopulation();
    }

    void Update()
    {
        if (!simulationFinished && AllAgentsDead())
        {
            BreedNewGeneration();
        }

        if (bestAgentsUI != null)
        {
            bestAgentsUI.agents = population;
        }
    }

    void CreateInitialPopulation()
    {
        population.Clear();
        for (int i = 0; i < populationSize; i++)
        {
            GameObject obj = Instantiate(dinoPrefab, spawnPoint.position, Quaternion.identity);
            DinoAgent agent = obj.GetComponent<DinoAgent>();
            agent.Initialize(Genome.RandomGenome());
            population.Add(agent);
        }

        UpdateUI();
    }

    bool AllAgentsDead()
    {
        return population.All(a => !a.isAlive);
    }

    void BreedNewGeneration()
    {
        generation++;

        if (obstacleManager != null)
        {
            obstacleManager.ClearObstacles();
        }

        List<DinoAgent> top = population.OrderByDescending(a => a.fitness).Take(parentsToSelect).ToList();

        string parentInfo = string.Join("\n", top.Select(p =>
            $"RT:{p.genome.reactionTime:F2} | TH:{p.genome.jumpThreshold:F2}"));

        List<DinoAgent> newPopulation = new List<DinoAgent>();

        foreach (DinoAgent a in population)
        {
            Genome parent1 = top[Random.Range(0, top.Count)].genome;
            Genome parent2 = top[Random.Range(0, top.Count)].genome;

            Genome childGenome = Genome.Crossover(parent1, parent2);
            childGenome.Mutate(mutationRate);

            a.transform.position = spawnPoint.position;
            a.gameObject.SetActive(true);
            a.Initialize(childGenome);

            newPopulation.Add(a);
        }

        population = newPopulation;
        UpdateUI(parentInfo);
    }

    void UpdateUI(string parentInfo = "")
    {
        populationText.text = $"Generación: {generation}\nPoblación: {populationSize}";
        parentsText.text = $"Padres:\n{parentInfo}";
        mutationSlider.value = mutationRate;
    }

    void SetMutationRateFromSlider(float value)
    {
        mutationRate = value;
    }

    public List<DinoAgent> GetAgents()
    {
        return population;
    }

    public void FinishSimulation()
    {
        Debug.Log("✅ Todos los agentes han llegado a la ronda objetivo. Simulación finalizada.");

        // Ejemplo de resumen con colores usando Rich Text para TextMeshPro
        string resumen =
            $"<color=#00CED1><b>Simulación finalizada</b></color>\n" +
            $"<color=#1E90FF>Generación:</color> <color=#FFFFFF>{generation}</color>\n" +
            $"<color=#32CD32>Población total:</color> <color=#FFFFFF>{populationSize}</color>\n" +
            $"<color=#FFA500>Tasa de mutación:</color> <color=#FFFFFF>{mutationRate:P1}</color>\n" +
            $"<color=#FF4500>Padres seleccionados:</color>\n";

        // Agregamos info de padres
        var topParents = population.OrderByDescending(a => a.fitness).Take(parentsToSelect);
        int i = 1;
        foreach (var parent in topParents)
        {
            resumen += $"  <color=#FFD700>Padre {i}:</color> " +
                       $"<color=#FFFFFF>RT={parent.genome.reactionTime:F2}, " +
                       $"TH={parent.genome.jumpThreshold:F2}, " +
                       $"LowJump={parent.genome.lowJumpForce:F1}, " +
                       $"MidJump={parent.genome.midJumpForce:F1}, " +
                       $"HighJump={parent.genome.highJumpForce:F1}</color>\n";
            i++;
        }

        if (simulationEndUI != null)
        {
            simulationEndUI.Show(resumen);
        }
    }
}