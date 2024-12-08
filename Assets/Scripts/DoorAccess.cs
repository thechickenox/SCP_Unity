using UnityEngine;

public class DoorAccess : MonoBehaviour
{
    public string requiredCardType; // Tipo de tarjeta que abre esta puerta
    public GameObject interactMessage; // Mensaje que se muestra al acercarse
    public KeyCode interactKey = KeyCode.Q; // Tecla para interactuar con la puerta

    private bool playerInRange = false;
    private BoxCollider2D doorCollider; // El collider físico de la puerta

    private void Start()
    {
        if (interactMessage != null)
        {
            interactMessage.SetActive(false); // Ocultar el mensaje inicialmente
        }

        // Obtener el BoxCollider2D de la puerta
        doorCollider = GetComponent<BoxCollider2D>();
        if (doorCollider == null)
        {
            Debug.LogError("No se encontró un BoxCollider2D en la puerta.");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            if (InventorySystem.instance.HasCard(requiredCardType))
            {
                OpenDoor();
            }
            else
            {
                Debug.Log("Necesitas la tarjeta " + requiredCardType + " para abrir esta puerta.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactMessage != null)
            {
                interactMessage.SetActive(true); // Mostrar el mensaje
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactMessage != null)
            {
                interactMessage.SetActive(false); // Ocultar el mensaje
            }
        }
    }

    private void OpenDoor()
    {
        // Mostrar un mensaje y desactivar el collider para abrir la puerta
        Debug.Log("Puerta abierta con la tarjeta " + requiredCardType);

        // Desactivar el collider para permitir el paso
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        // Aquí puedes agregar efectos, animaciones o desactivar el objeto de la puerta
        Destroy(gameObject, 2f); // Opcional: Destruir la puerta tras un tiempo
    }
}