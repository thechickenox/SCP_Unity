using UnityEngine;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance; // Singleton

    private HashSet<string> collectedCards = new HashSet<string>(); // Solo guarda tipos únicos

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persistir el inventario entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para agregar una tarjeta al inventario
    public void AddCard(string cardType)
    {
        if (!collectedCards.Contains(cardType))
        {
            collectedCards.Add(cardType);
            Debug.Log("Tarjeta agregada: " + cardType);
        }
    }

    // Método para verificar si se tiene una tarjeta específica
    public bool HasCard(string cardType)
    {
        return collectedCards.Contains(cardType);
    }
}