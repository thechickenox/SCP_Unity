using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasSequenceManager : MonoBehaviour
{
    public GameObject bgCanvas;  // Primer Canvas a mostrar
    public GameObject firstCanvas;  // Primer Canvas a mostrar
    public GameObject secondCanvas; // Segundo Canvas a mostrar
    public float secondCanvasDuration = 3f; // Duración del segundo Canvas en segundos
    public float fadeDuration = 0.5f; // Duración del fade in y fade out
    public string nextSceneName = "NextScene"; // Nombre de la escena a cargar

    private bool isSequenceRunning = false; // Control para evitar múltiples activaciones
    private CanvasGroup firstCanvasGroup;
    private CanvasGroup secondCanvasGroup;
    private CanvasGroup bgCanvasGroup;

    private void Start()
    {
        // Configurar CanvasGroup para el fade in/out
        firstCanvasGroup = SetupCanvasGroup(firstCanvas);
        secondCanvasGroup = SetupCanvasGroup(secondCanvas);
        bgCanvasGroup = SetupCanvasGroup(bgCanvas);
    }

    public void StartSequence()
    {
        if (!isSequenceRunning) // Evitar múltiples activaciones
        {
            isSequenceRunning = true;
            StartCoroutine(ShowCanvasesAndLoadScene());
        }
    }

    private CanvasGroup SetupCanvasGroup(GameObject canvas)
    {
        if (canvas != null)
        {
            CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = canvas.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f; // Comienza invisible
            canvas.SetActive(false); // Asegúrate de que está desactivado inicialmente
            return canvasGroup;
        }
        return null;
    }

    private IEnumerator ShowCanvasesAndLoadScene()
    {   
firstCanvas.SetActive(true); // Desactivarlo tras el fade out
        yield return StartCoroutine(FadeIn(bgCanvasGroup));
        // Mostrar el primer Canvas con fade in
        if (firstCanvasGroup != null)
        {
            yield return StartCoroutine(FadeIn(firstCanvasGroup));
        }

        // Esperar un breve momento antes de mostrar el segundo Canvas
        yield return new WaitForSeconds(0.5f);

        // Mostrar el segundo Canvas con fade in
        if (secondCanvasGroup != null)
        {
            if (firstCanvasGroup != null)
            {
                yield return StartCoroutine(FadeOut(firstCanvasGroup)); // Ocultar el primero
                firstCanvas.SetActive(false); // Desactivarlo tras el fade out
            }
            secondCanvas.SetActive(true); // Activar el segundo
            yield return StartCoroutine(FadeIn(secondCanvasGroup));
        }

        // Esperar la duración indicada para el segundo Canvas
        yield return new WaitForSeconds(secondCanvasDuration);

        // Desvanecer el segundo Canvas antes de cambiar de escena
        if (secondCanvasGroup != null)
        {
            yield return StartCoroutine(FadeOut(secondCanvasGroup));
            secondCanvas.SetActive(false);
        }

        // Cargar la siguiente escena
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        canvasGroup.gameObject.SetActive(true); // Activar el Canvas
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}