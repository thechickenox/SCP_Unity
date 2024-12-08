using UnityEngine;
using TMPro; // Solo si usas TextMeshPro
using System.Collections;

public class TriggerTextDisplay : MonoBehaviour
{
    public GameObject textObject; // El objeto de texto en la UI
    public float fadeDuration = 0.5f; // Duración del fade in y fade out

    private CanvasGroup canvasGroup;

    private void Start()
    {
        // Asegúrate de que el CanvasGroup esté asignado y oculto al inicio
        if (textObject != null)
        {
            canvasGroup = textObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = textObject.AddComponent<CanvasGroup>();
            }

            // Inicializa el CanvasGroup al estado invisible
            canvasGroup.alpha = 0f;
            textObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && textObject != null)
        {
            // Evita reiniciar el fade si ya está visible
            if (canvasGroup.alpha == 1f) return;

            textObject.SetActive(true);
            StartCoroutine(FadeIn());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && textObject != null)
        {
            // Evita reiniciar el fade si ya está invisible
            if (canvasGroup.alpha == 0f) return;

            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        // Asegúrate de que el alpha comienza desde 0
        canvasGroup.alpha = 0f;

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
        textObject.SetActive(false); // Ocultar el texto al finalizar el fade out
    }
}