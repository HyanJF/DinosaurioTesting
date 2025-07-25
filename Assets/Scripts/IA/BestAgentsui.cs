using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BestAgentsui : MonoBehaviour
{
    public TextMeshProUGUI infoText;      // Top 3 info
    public TextMeshProUGUI namesText;     // Lista completa con colores
    public List<DinoAgent> agents;

    void Update()
    {
        if (agents == null || agents.Count == 0)
        {
            infoText.text = "<color=red>No hay agentes.</color>";
            namesText.text = "";
            return;
        }

        var aliveAgents = agents.Where(a => a.isAlive).ToList();

        if (aliveAgents.Count == 0)
        {
            infoText.text = "<color=gray>Todos murieron.</color>";
        }
        else
        {
            var top3 = aliveAgents.OrderByDescending(a => a.fitness).Take(3).ToList();

            string[] topColors = { "#00FF00", "#00CED1", "#FF69B4" }; // verde, cyan, rosa

            string info = "<b><color=#FFD700>Top 3 Agentes Vivos</color></b>\n\n";

            for (int i = 0; i < top3.Count; i++)
            {
                var a = top3[i];
                info += $"<color={topColors[i]}><b>#{i + 1}</b></color> - " +
                        $"<color=white>Fitness:</color> <color={topColors[i]}>{a.fitness:F1}</color> | " +
                        $"<color=white>Reacción:</color> <color={topColors[i]}>{a.genome.reactionTime:F2}</color> | " +
                        $"<color=white>Umbral:</color> <color={topColors[i]}>{a.genome.jumpThreshold:F1}</color>\n";
            }

            infoText.text = info;
        }

        // Mostrar lista completa de nombres con estado de vida
        string[] allColors = { "#00FF00", "#00CED1", "#FF69B4", "#FFA500", "#9400D3", "#1E90FF", "#FF4500", "#32CD32" };
        string namesOutput = "<b><color=white>Agentes:</color></b>\n";

        for (int i = 0; i < agents.Count; i++)
        {
            var a = agents[i];
            string color = a.isAlive ? allColors[i % allColors.Length] : "black";
            namesOutput += $"<color={color}>• {a.name}</color>\n";
        }

        namesText.text = namesOutput;
    }
}
