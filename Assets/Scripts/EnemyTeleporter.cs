using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public Transform player;                  // Referencia al transform del jugador
    public float detectionRange = 5f;         // Rango de detección para atrapar al jugador
    public float detectionMoveRange = 10f;     // Rango de detección para detenerse cuando el jugador está cerca
    public float moveDistance = 3f;           // Distancia que el enemigo se moverá de lado a lado
    public float moveSpeed = 2f;              // Velocidad de movimiento del enemigo
    public float initialScaleX = 0f;              // Velocidad de movimiento del enemigo
    public Vector3 offset = new Vector3(1.5f, 0, 0); // Offset para aparecer al lado del jugador al atraparlo

    private bool isMovingRight = true;        // Dirección de movimiento del enemigo
    private float startPositionX;             // Posición inicial en X para calcular la distancia de movimiento
    private bool playerCaught = false;        // Indica si el jugador fue atrapado

    void Start()
    {
        startPositionX = transform.position.x; // Guardamos la posición inicial en X
    }

    void Update()
    {
        // Calcular la distancia entre el enemigo y el jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Verificar si el jugador está dentro del rango para detenerse
        if (distanceToPlayer <= detectionMoveRange && !playerCaught)
        {
            // Si el jugador está en el rango de detección para "atrapar"
            if (distanceToPlayer <= detectionRange)
            {
                CatchPlayer();
            }
            return; // Salir de la función Update para detener el movimiento
        }

        // Mover al enemigo de lado a lado si el jugador está fuera del rango de detección
        MoveHorizontally();
    }

    void MoveHorizontally()
    {
        // Verificar la dirección y calcular la nueva posición
        if (isMovingRight)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            transform.localScale = new Vector3(-initialScaleX, transform.localScale.y, transform.localScale.z);
            
            // Cambiar la dirección si ha alcanzado la distancia máxima a la derecha
            if (transform.position.x >= startPositionX + moveDistance)
            {
                isMovingRight = false;
            }
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            transform.localScale = new Vector3(initialScaleX, transform.localScale.y, transform.localScale.z);
            
            // Cambiar la dirección si ha alcanzado la distancia máxima a la izquierda
            if (transform.position.x <= startPositionX - moveDistance)
            {
                isMovingRight = true;
            }
        }
    }

    void CatchPlayer()
    {
        playerCaught = true;

        // Teletransportar al enemigo junto al jugador con el offset
        transform.position = player.position + offset;

        // Aquí puedes agregar una animación o efecto para mostrar que el jugador "murió"
        Debug.Log("Jugador atrapado!");

        // Deshabilitar al jugador (ejemplo)
        player.gameObject.SetActive(false);

        // Mostrar la pantalla de Game Over llamando al script de Game Over
        FindAnyObjectByType<GameOver2>().MostrarGameOver();
    }

    void RespawnPlayer()
    {
        // "Revivir" al jugador y restablecer las variables
        player.gameObject.SetActive(true);
        playerCaught = false;
        Debug.Log("Jugador revivido!");
    }
}