using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [SerializeField] private AudioClip enemySoundClip; // Sonido que emitirá el enemigo
    [SerializeField] private Transform player; // Referencia al jugador
    [SerializeField] private float maxDistance = 20f; // Distancia máxima para escuchar el sonido
    [SerializeField] private float volumeAtClosest = 1f; // Volumen cuando el jugador está más cerca

    private AudioSource audioSource;

    void Start()
    {
        // Configurar el AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = enemySoundClip;
        audioSource.loop = true; // El sonido estará en loop
        audioSource.spatialBlend = 1f; // Hacer que el sonido sea 3D
        audioSource.rolloffMode = AudioRolloffMode.Linear; // Rolloff lineal para un desvanecimiento realista
        audioSource.maxDistance = maxDistance; // Definir la distancia máxima de audición
        audioSource.Play(); // Comenzar a reproducir el sonido
    }

    void Update()
    {
        if (player == null) return;

        // Calcular la distancia entre el enemigo y el jugador
        float distance = Vector3.Distance(transform.position, player.position);

        // Ajustar el volumen según la distancia
        float volume = Mathf.Clamp01(1 - (distance / maxDistance)) * volumeAtClosest;
        audioSource.volume = volume;

        // Opcional: Debug para visualizar el rango de sonido
        Debug.DrawLine(transform.position, player.position, Color.red);
    }
}