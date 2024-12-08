using UnityEngine;
using System.Collections;

public class ZoneAudioTrigger : MonoBehaviour
{
    public AudioSource[] audioSourcesToStop; // Array de AudioSources que se detendrán
    public AudioSource newAudioSource; // Nuevo AudioSource que reproducirá el sonido
    public float waitTime = 2f; // Tiempo de espera antes de reproducir el nuevo sonido

    public void StopAudiosAndPlayNew()
    {
        // Detener todos los AudioSources en el array
        foreach (AudioSource audioSource in audioSourcesToStop)
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        // Reproducir el nuevo audio
        if (newAudioSource != null)
        {
            newAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("El nuevo AudioSource no está asignado.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Asegúrate de usar el tag correcto
        {
            StartCoroutine(TriggerAudioWithDelay());
        }
    }

    private IEnumerator TriggerAudioWithDelay()
    {
        yield return new WaitForSeconds(waitTime); // Esperar antes de reproducir el nuevo audio
        StopAudiosAndPlayNew();
    }
}