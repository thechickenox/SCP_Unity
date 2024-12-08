using UnityEngine;
using System.Collections;

public class HazardZone : MonoBehaviour
{
    [SerializeField] Transform player; // Referencia al jugador
    public string[] requiredItems; // Lista de elementos requeridos
    public GameObject warningPanel; // Panel que se activa si faltan elementos
    public GameObject deathPanel; // Panel que se activa al morir
    public GameObject damagePanel; // Panel que parpadea cuando recibe daño

    private bool playerInRange = false;
    private float timeInZone = 0f; // Tiempo dentro de la zona peligrosa
    private bool hasAllItems = false;
    private bool isVibrating = false; // Controla si el daño está vibrando
    private bool panelVisible = false; // Controla si el panel de advertencia está visible

    private void Start()
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
        }
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
        if (damagePanel != null)
        {
            damagePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (!hasAllItems)
            {
                timeInZone += Time.deltaTime;

                // Si no tiene todos los elementos, se muestra el warning panel y se inicia la vibración
                if (!isVibrating && timeInZone >= 1f) // Start after 1 second
                {
                    ShowWarningPanel();
                }

                // Mostrar damage panel parpadeando mientras está en la zona peligrosa
                if (damagePanel != null && !deathPanel.activeSelf) // No mostrar damage si ya está muerto
                {
                    StartCoroutine(FlickerDamagePanel());
                }

                // Trigger death panel after 4 seconds inside the zone
                if (timeInZone >= 4f)
                {
                    KillPlayer();
                }
            }
        }
        else
        {
            timeInZone = 0f; // Reset time when outside the zone
            // Hide warning panel if player exits the zone
            if (warningPanel != null)
            {
                warningPanel.SetActive(false);
            }

            // Detener la vibración si el jugador sale
            if (isVibrating)
            {
                isVibrating = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            CheckItems(); // Check items when entering zone

            // Reset time if player enters the zone and doesn't have required items
            if (!hasAllItems)
            {
                timeInZone = 0f; // Reset the timer
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;

            // Detener la vibración cuando el jugador sale de la zona
            if (isVibrating)
            {
                isVibrating = false;
            }

            // Reiniciar el tiempo en zona al salir
            timeInZone = 0f;

            // Asegurarnos de que el panel de advertencia esté oculto al salir
            if (warningPanel != null)
            {
                warningPanel.SetActive(false);
            }

            // Detener el parpadeo del damage panel
            if (damagePanel != null)
            {
                StopCoroutine(FlickerDamagePanel());
                damagePanel.SetActive(false);
            }
        }
    }

    private void CheckItems()
    {
        hasAllItems = true; // Asumimos que el jugador tiene todos los elementos

        foreach (string item in requiredItems)
        {
            if (!InventorySystem.instance.HasCard(item))
            {
                hasAllItems = false;
                break;
            }
        }

        if (hasAllItems)
        {
            Debug.Log("Todos los elementos están presentes. Zona segura.");
        }
        else
        {
            Debug.Log("No tienes los elementos necesarios. Zona peligrosa.");
        }
    }

    private void ShowWarningPanel()
    {
        if (warningPanel != null && !panelVisible)
        {
            panelVisible = true;
            warningPanel.SetActive(true);

            // Ocultar el panel después de 0.5 segundos
            Invoke(nameof(HideWarningPanel), 0.5f);
        }
    }

    private void HideWarningPanel()
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
            panelVisible = false;
        }
    }
    void KillPlayer()
    {
        player.gameObject.SetActive(false); // Deshabilitar al jugador
        FindAnyObjectByType<GameOver2>().MostrarGameOver();
    }

    private IEnumerator FlickerDamagePanel()
    {
        while (playerInRange && !deathPanel.activeSelf) // Continuar parpadeando hasta morir
        {
            if (damagePanel != null)
            {
                damagePanel.SetActive(!damagePanel.activeSelf); // Toggle panel visibility
            }
            yield return new WaitForSeconds(0.5f); // Parpadeo cada 0.5 segundos
        }

        if (damagePanel != null)
        {
            damagePanel.SetActive(false); // Asegurarse de que se apague al salir
        }
    }
}