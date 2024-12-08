using UnityEngine;
using TMPro; // Solo si usas TextMeshPro
using System.Collections;

public class SequentialTextAndFlashingLight : MonoBehaviour
{
    [Header("Text Settings")]
    public GameObject[] textObjects; // Lista de objetos de texto a mostrar
    public float fadeDuration = 0.5f; // Duración del fade in y fade out
    public float displayDuration = 1.5f; // Tiempo que cada texto permanece visible
    public float delayBetweenTexts = 0.5f; // Tiempo entre textos

    [Header("Light Settings")]
    public GameObject lightObject; // Objeto de luz 2D
    public float lightOnDuration = 2f; // Tiempo que la luz permanece encendida
    public float flashingDuration = 3f; // Tiempo que la luz parpadea
    public float flashInterval = 0.2f; // Intervalo entre flashes
    public float lightDelay = 5f; // Tiempo antes de que comience a parpadear la luz
    public Color normalColor = Color.white; // Color inicial de la luz
    public Color flashingColor = Color.red; // Color de la luz al parpadear

    private CanvasGroup[] canvasGroups;
    private UnityEngine.Rendering.Universal.Light2D light2D;

    private void Start()
    {
        // Configurar CanvasGroups para textos
        canvasGroups = new CanvasGroup[textObjects.Length];
        for (int i = 0; i < textObjects.Length; i++)
        {
            if (textObjects[i] != null)
            {
                canvasGroups[i] = textObjects[i].GetComponent<CanvasGroup>();
                if (canvasGroups[i] == null)
                {
                    canvasGroups[i] = textObjects[i].AddComponent<CanvasGroup>();
                }
                canvasGroups[i].alpha = 0f;
                textObjects[i].SetActive(false);
            }
        }

        // Configurar luz 2D
        if (lightObject != null)
        {
            light2D = lightObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            if (light2D != null)
            {
                light2D.color = normalColor;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DisplayTextsAndControlLight());
        }
    }

    private IEnumerator DisplayTextsAndControlLight()
    {
        // Iniciar la secuencia de textos de inmediato
        foreach (var textObject in textObjects)
        {
            if (textObject != null)
            {
                textObject.SetActive(true);
                CanvasGroup canvasGroup = textObject.GetComponent<CanvasGroup>();

                // Fade in
                yield return StartCoroutine(Fade(canvasGroup, 0f, 1f));

                // Esperar mientras el texto está visible
                yield return new WaitForSeconds(displayDuration);

                // Fade out
                yield return StartCoroutine(Fade(canvasGroup, 1f, 0f));

                textObject.SetActive(false);

                // Esperar entre textos
                yield return new WaitForSeconds(delayBetweenTexts);
            }
        }

        // Esperar el tiempo antes de que comience a parpadear la luz
        yield return new WaitForSeconds(lightDelay);

        // Encender la luz
        if (lightObject != null)
        {
            lightObject.SetActive(true);

            // Esperar el tiempo que debe estar encendida
            yield return new WaitForSeconds(lightOnDuration);

            // Comenzar parpadeo
            if (light2D != null)
            {
                yield return StartCoroutine(FlashLight());
            }
        }

        // Apagar la luz al finalizar
        if (lightObject != null)
        {
            lightObject.SetActive(false);
        }
    }

    private IEnumerator FlashLight()
    {
        float elapsedTime = 0f;
        while (elapsedTime < flashingDuration)
        {
            // Alternar entre el color normal y el color de parpadeo
            light2D.color = light2D.color == normalColor ? flashingColor : normalColor;
            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval;
        }

        // Restaurar el color original de la luz
        light2D.color = normalColor;
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }
}