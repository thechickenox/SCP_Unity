using UnityEngine;
using UnityEngine.UI;

public class CardCollector : MonoBehaviour
{
    public GameObject interactionText; // Texto de interacción para recoger tarjeta
    private AccessCard nearbyCard;

    private void Update()
    {
        if (nearbyCard != null && Input.GetKeyDown(KeyCode.X))
        {
            InventorySystem.instance.AddCard(nearbyCard.cardType);
            Destroy(nearbyCard.gameObject); // Elimina la tarjeta del juego
            interactionText.gameObject.SetActive(false); // Oculta el mensaje de interacción
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AccessCard card = collision.GetComponent<AccessCard>();
        if (card != null)
        {
            nearbyCard = card;
            interactionText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        AccessCard card = collision.GetComponent<AccessCard>();
        if (card != null && card == nearbyCard)
        {
            nearbyCard = null;
            interactionText.SetActive(false);
        }
    }
}