using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuFunctions : MonoBehaviour
{
    public string mainMenuSceneName = "Menu"; // Nombre de la escena del menú principal

    // Función para reiniciar el juego
    public void RestartGame()
    {
        Time.timeScale = 1f; // Reanuda el tiempo por si estaba pausado
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Carga la escena actual
    }

    // Función para ir al menú principal
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Reanuda el tiempo
        SceneManager.LoadScene(mainMenuSceneName); // Carga la escena del menú principal
    }
}