using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneTrigger : MonoBehaviour
{
    [Tooltip("Nombre de la siguiente escena que se cargará")]
    public string nextSceneName;

    [Tooltip("Tiempo de espera antes de cargar la siguiente escena (en segundos)")]
    public float delayBeforeLoad = 2f;

    [Tooltip("Tag del objeto que activará el cambio de escena")]
    public string triggeringTag = "Player";

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTriggered && collision.CompareTag(triggeringTag))
        {
            hasTriggered = true; // Evitar múltiples activaciones
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(nextSceneName);
    }
}