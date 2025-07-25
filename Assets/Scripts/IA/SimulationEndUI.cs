using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SimulationEndUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI summaryText;
    public GameObject quitarpanel;
    public Button restartButton;
    public Button exitButton;

    private void Awake()
    {
        panel.SetActive(false); // Oculto al inicio

        restartButton.onClick.AddListener(OnRestartClicked);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    // Mostrar UI al finalizar simulación con resumen
    public void Show(string summary)
    {
        quitarpanel.SetActive(false);
        panel.SetActive(true);
        summaryText.text = summary;
    }

    // Reinicia la escena actual para volver a correr la simulación
    private void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Cierra la aplicación o vuelve a menú (según plataforma)
    private void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
