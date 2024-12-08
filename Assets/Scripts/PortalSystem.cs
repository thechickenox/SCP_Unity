using UnityEngine;

public class PortalSystem : MonoBehaviour
{
    [Header("Portal Configuration")]
    public Transform targetPortal; // El portal al que te llevará
    public GameObject messageActive; // Mensaje que se muestra al interactuar
    public KeyCode interactKey = KeyCode.Q; // Tecla para interactuar

    private bool isPlayerNearby = false; // Si el jugador está cerca

    void Update()
    {
        // Verifica si el jugador está cerca y presiona la tecla de interacción
        if (isPlayerNearby && Input.GetKeyDown(interactKey))
        {
            TeleportPlayer(); // Mueve al jugador al portal destino
        }
    }

    private void TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && targetPortal != null)
        {
            player.transform.position = targetPortal.position; // Mueve al jugador al portal destino
        }
    }

    private void ShowMessage(GameObject message)
    {
        message.SetActive(true); // Activar el mensaje
        Invoke("HideMessage", 2f); // Ocultar el mensaje después de 2 segundos
    }

    private void HideMessage()
    {
        if (messageActive != null)
        {
            messageActive.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Mostrar mensaje de éxito
            if (messageActive != null)
            {
                ShowMessage(messageActive);
            }
            isPlayerNearby = true; // El jugador está dentro del rango del portal
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HideMessage();
            isPlayerNearby = false; // El jugador salió del rango del portal
        }
    }
}