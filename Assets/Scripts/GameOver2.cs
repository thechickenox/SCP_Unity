using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameOver2 : MonoBehaviour
{
    public GameObject objectivePanel;
    public GameObject gameOverPanel;
    public float fadeDuration = 1f; // Duración del efecto de fade
    public AudioClip gameOverSound; // Clip de sonido de Game Over
    private AudioSource audioSource;
    private CanvasGroup canvasGroup;
    public float gameOverVolume = 0.5f; // Volumen del sonido de Game Over (de 0 a 1)

    private void Start()
    {
        // Configurar el AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configurar el CanvasGroup
        canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameOverPanel.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
        gameOverPanel.SetActive(false);
    }

    public void MostrarGameOver()
    {
        objectivePanel.SetActive(false);
        StartCoroutine(FadeIn());
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);

        // Configurar y reproducir sonido de Game Over
        if (gameOverSound != null && audioSource != null)
        {
            audioSource.volume = gameOverVolume; // Ajusta el volumen directamente
            audioSource.PlayOneShot(gameOverSound);
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}