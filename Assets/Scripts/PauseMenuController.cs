using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Asigna el panel del menú de pausa desde el inspector
    private bool isPaused = false;

    void Update()
    {
        // Detecta si se presiona la tecla Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pausa el tiempo en el juego
        pauseMenuPanel.SetActive(true); // Muestra el menú de pausa
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Reanuda el tiempo
        pauseMenuPanel.SetActive(false); // Oculta el menú de pausa
    }
}