using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] Transform player; // Referencia al jugador
    public float activationDelay = 2f; // Retraso antes de activar la persecución
    public GameObject deathPanel; // Referencia al panel de muerte
    public Animator enemyAnimator; // Referencia al Animator
    public float killingDistance = 2f; // Distancia para "matar" al jugador
    public AudioSource audioSource; // Referencia al AudioSource
    public AudioClip chaseSound; // Clip de sonido para la persecución

    private NavMeshAgent agent;
    private bool isActivated = false; // Controla si el enemigo está activado
    private bool isPlayerInRange = false; // Controla si el jugador está en rango
    private float activationTimer; // Temporizador para el retraso de activación

    private float navMeshUpdateInterval = 0.2f; // Intervalo de actualización del destino
    private float navMeshUpdateTimer = 0f;

    void Start()
    {
        Time.timeScale = 1; // Pausar el juego
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;  // No rotar automáticamente el agente
        agent.updateUpAxis = false;    // No modificar el eje Y
        activationTimer = activationDelay;


        // Asegurarse de que hay un AudioSource en el objeto
        if (audioSource == null)
        {
            // Configurar el AudioSource para que repita el sonido
            audioSource.loop = true;
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Activar enemigo si el jugador está en rango
        if (isPlayerInRange && !isActivated)
        {
            activationTimer -= Time.deltaTime;

            if (enemyAnimator != null)
            {
                enemyAnimator.SetBool("active", true); // Activar la animación
            }

            if (activationTimer <= 0f)
            {
                isActivated = true;
                StartCoroutine(WaitForAnimationAndStartChase()); // Esperar la animación

                // Reproducir sonido al activar la persecución
                if (audioSource != null && chaseSound != null)
                {
                    audioSource.PlayOneShot(chaseSound); // Reproduce el sonido
                }
            }
        }

        // Persecución continua del jugador
        if (isActivated)
        {
            navMeshUpdateTimer += Time.deltaTime;
            if (navMeshUpdateTimer >= navMeshUpdateInterval)
            {
                UpdateAgentDestination(); // Actualizar destino constantemente
                navMeshUpdateTimer = 0f;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= killingDistance)
            {
                KillPlayer();
            }
        }
    }

    private void UpdateAgentDestination()
    {
        // Establecer el destino al jugador, sin restricciones de distancia
        if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void KillPlayer()
    {
        player.gameObject.SetActive(false); // Deshabilitar al jugador
        FindAnyObjectByType<GameOver2>().MostrarGameOver(); // Mostrar la pantalla de game over
        Time.timeScale = 0; // Pausar el juego
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true; // El jugador entra en el rango
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false; // El jugador sale del rango
        }
    }

    private IEnumerator WaitForAnimationAndStartChase()
    {
        yield return new WaitForSeconds(2f); // Tiempo de espera para la animación
        UpdateAgentDestination(); // Comienza a perseguir al jugador
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, killingDistance); // Rango de "muerte"
    }
}