# Dino Genetic Jump Simulation

## Descripción

Este proyecto es una simulación basada en agentes que intentan superar obstáculos con diferentes alturas, controlados por una inteligencia artificial evolucionada mediante algoritmos genéticos. Cada agente (DinoAgent) tiene un genoma que determina su capacidad de salto, tiempo de reacción, y otros parámetros, y evoluciona generación tras generación para mejorar su desempeño.

La simulación incluye:

- Generación de obstáculos con distintas alturas y velocidad creciente.
- Control de agentes con diferentes genomas para decidir cuándo y cómo saltar.
- Algoritmo genético que cruza y muta los genomas para mejorar la población.
- UI para visualizar generación actual, padres seleccionados, tasa de mutación y ronda actual.
- Finalización automática al alcanzar una ronda meta.
- Panel resumen al finalizar la simulación con estadísticas y opciones para reiniciar o salir.

---

## Estructura de Carpetas y Scripts

- **Scripts**
  - `DinoAgent.cs`  
    Controla el comportamiento de cada agente, incluyendo salto, detección de obstáculos, caída acelerada y actualización de UI individual.
  
  - `Genome.cs`  
    Clase que define el genoma de cada agente con parámetros para tiempo de reacción, fuerza de salto en distintas alturas y distancia óptima para saltar. Incluye métodos para cruzar (crossover) y mutar genes.

  - `GeneticAlgorithmManager.cs`  
    Administra la población de agentes, controla las generaciones, selección de padres, cruce, mutación y actualización de UI general. Detecta el fin de la simulación.

  - `ObstacleManager.cs`  
    Genera obstáculos periódicamente con dificultad creciente, controla las rondas y notifica cuando se alcanza la ronda máxima. Actualiza UI con la ronda actual con un simple efecto de fade en el texto.

  - `SimulationEndUI.cs`  
    Panel que aparece al finalizar la simulación mostrando un resumen con colores, y botones para reiniciar la simulación o salir.

- **Prefabs**
  - Prefabs para cada tipo de obstáculo (`lowObstaclePrefab`, `midObstaclePrefab`, `highObstaclePrefab`).
  - Prefab para el agente (`dinoPrefab`).

- **UI**
  - Textos para población, generación, padres, tasa de mutación, ronda actual.
  - Panel de resumen final con texto enriquecido y botones.

---

## Funcionamiento

1. **Inicio:** Se crea una población inicial de agentes con genomas aleatorios.
2. **Simulación:** Los agentes corren y saltan obstáculos que aparecen desde un spawn fijo.
3. **Rondas:** Cada cierto tiempo, la velocidad y frecuencia de los obstáculos aumenta y se incrementa la ronda actual.
4. **Evaluación:** Cuando todos los agentes mueren o superan la ronda objetivo, la generación termina.
5. **Evolución:** Se seleccionan los mejores agentes, se cruzan y mutan sus genomas para crear una nueva población.
6. **Repetición:** El ciclo continúa hasta que se alcance la ronda máxima o se decida terminar.
7. **Resumen:** Al finalizar, se muestra un panel con estadísticas coloreadas y opciones para reiniciar o salir.

---

## Cómo usar

1. Importa el proyecto a Unity (versión 2020 o superior recomendada).
2. Configura los prefabs de agentes y obstáculos en los componentes `GeneticAlgorithmManager` y `ObstacleManager`.
3. Asigna los textos UI y paneles correspondientes.
4. Ejecuta la escena y observa cómo los agentes evolucionan para superar los obstáculos.
5. Usa el panel final para reiniciar la simulación o salir.

---

## Parámetros configurables

- **GeneticAlgorithmManager**
  - Tamaño de población.
  - Tasa de mutación.
  - Número de padres seleccionados.

- **ObstacleManager**
  - Intervalos iniciales y mínimos de spawn de obstáculos.
  - Velocidad inicial de obstáculos.
  - Número máximo de rondas.

- **Genome**
  - Intervalos para tiempo de reacción, fuerza de salto, distancia óptima (modificable en código o con mutación).

---

## Créditos

Desarrollado por Yeicok.P Studios.
