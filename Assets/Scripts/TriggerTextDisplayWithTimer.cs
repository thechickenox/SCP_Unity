using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con el botón
using TMPro; // Solo si usas TextMeshPro
using System.Collections;

public class TriggerTextDisplayWithTimer : MonoBehaviour
{
    public GameObject textObject; // El objeto de texto en la UI
    public Button closeButton; // Botón para cerrar el texto manualmente
    public float displayDuration = 3.0f; // Tiempo que el texto permanece visible
    public float fadeDuration = 0.5f; // Duración del fade in y fade out

    private CanvasGroup canvasGroup;
    private Coroutine fadeOutCoroutine;

    private void Start()
    {
        if (textObject != null)
        {
            canvasGroup = textObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = textObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f;
            textObject.SetActive(false);
        }

        if (closeButton != null)
        {
            closeButton.gameObject.SetActive(false); // Ocultar el botón al inicio
            closeButton.onClick.AddListener(HideTextManually); // Agregar evento de cierre
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && textObject != null)
        {
            textObject.SetActive(true);
            if (closeButton != null)
            {
                closeButton.gameObject.SetActive(true); // Mostrar botón de cierre
            }
            
            if (fadeOutCoroutine != null)
            {
                StopCoroutine(fadeOutCoroutine); // Detener cualquier fade out en progreso
            }

            StartCoroutine(FadeIn());

            // Iniciar una rutina para ocultar el texto después de cierto tiempo
            fadeOutCoroutine = StartCoroutine(HideTextAfterDelay(displayDuration));
        }
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(FadeOut());
    }

    private void HideTextManually()
    {
        // Detener la rutina de temporizador si el usuario cierra el texto manualmente
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        textObject.SetActive(false); // Ocultar el texto

        if (closeButton != null)
        {
            closeButton.gameObject.SetActive(false); // Ocultar el botón también
        }
    }
}
