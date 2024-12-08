using UnityEngine;

public class MusicZoneManager : MonoBehaviour
{
    public AudioClip defaultMusic;    // Música de fondo predeterminada
    public AudioClip zoneAMusic;      // Música para la Zona A
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = defaultMusic;
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.name == "ZonaA")
            {
                ChangeMusic(zoneAMusic);
            }
            // Puedes añadir más zonas aquí
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Vuelve a la música predeterminada al salir de la zona
            ChangeMusic(defaultMusic);
        }
    }

    private void ChangeMusic(AudioClip newClip)
    {
        if (audioSource.clip != newClip)
        {
            audioSource.Stop();
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}