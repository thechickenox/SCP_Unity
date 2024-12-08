using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    public Transform player;                  // Referencia al jugador
    public GameObject deathPanel;             // Panel de muerte
    public float moveDistance = 4f;           // Distancia de patrullaje
    public float moveSpeed = 2f;              // Velocidad de patrullaje
    public float detectionRange = 3f;         // Rango de detección para atrapar al jugador
    public float initialScaleX = 1f;          // Escala inicial en el eje X (para girar el sprite)
    public Vector3 offset = new Vector3(1.5f, 0, 0); // Offset al atrapar al jugador

    private bool isMovingRight = true;        // Dirección actual del movimiento
    private float startPositionX;             // Posición inicial en X

    void Start()
    {
        startPositionX = transform.position.x; // Guardar la posición inicial
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Verificar si el jugador está en rango para atrapar
        if (distanceToPlayer <= detectionRange)
        {
            CatchPlayer();
            return;
        }

        // Realizar patrullaje horizontal
        MoveHorizontally();
    }

    void MoveHorizontally()
    {
        if (isMovingRight)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            transform.localScale = new Vector3(initialScaleX, transform.localScale.y, transform.localScale.z);

            // Cambiar de dirección al alcanzar la distancia máxima
            if (transform.position.x >= startPositionX + moveDistance)
            {
                isMovingRight = false;
            }
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            transform.localScale = new Vector3(-initialScaleX, transform.localScale.y, transform.localScale.z);

            // Cambiar de dirección al alcanzar la distancia mínima
            if (transform.position.x <= startPositionX - moveDistance)
            {
                isMovingRight = true;
            }
        }
    }

    void CatchPlayer()
    {
        // Teletransportar al enemigo junto al jugador con el offset
        transform.position = player.position + offset;

        // Deshabilitar al jugador
        player.gameObject.SetActive(false);

        // Mostrar el panel de muerte
        deathPanel.SetActive(true);

        // Pausar el juego
        Time.timeScale = 0;
    }
}