using UnityEngine;

public class CharacterPath : MonoBehaviour
{
    public Transform[] waypoints; // Arrastra los waypoints aquí
    public float speed = 2f; // Velocidad del personaje
    private int currentWaypoint = 0; // Índice del waypoint actual
    public Animator animator; // Controlador de animaciones

    public float[] waitTimes; // Tiempos de espera en cada waypoint
    private bool isWaiting = false; // Flag para controlar la espera

    public float initialScaleX = 0.66412f; // Valor inicial del scale en el eje X

    void Update()
    {
        if (!isWaiting && currentWaypoint < waypoints.Length)
        {
            // Obtener el waypoint objetivo
            Transform target = waypoints[currentWaypoint];

            // Calcular la dirección hacia el waypoint
            Vector2 direction = target.position - transform.position;

            // Cambiar la escala del personaje según la dirección horizontal
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(-initialScaleX, transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(initialScaleX, transform.localScale.y, transform.localScale.z);
            }

            // Mover al personaje hacia el waypoint
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Actualizar el parámetro "Movement" del Animator
            animator.SetFloat("Movement", speed);

            // Verificar si el personaje llegó al waypoint
            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                StartCoroutine(WaitAtWaypoint());
            }
        }
        else if (currentWaypoint >= waypoints.Length)
        {
            // Si ya no hay waypoints, detener la animación
            animator.SetFloat("Movement", 0f);
        }
    }

    private System.Collections.IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;

        // Detener la animación y la velocidad del personaje
        animator.SetFloat("Movement", 0f);

        // Esperar el tiempo configurado para este waypoint (si existe)
        if (waitTimes.Length > currentWaypoint)
        {
            yield return new WaitForSeconds(waitTimes[currentWaypoint]);
        }

        // Avanzar al siguiente waypoint después de esperar
        currentWaypoint++;
        isWaiting = false;
    }
}