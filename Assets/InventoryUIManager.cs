using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Para usar TextMeshPro

public class InventoryUIManager : MonoBehaviour
{
    // Estructura para asociar texto e imagen de cada tarjeta
    [System.Serializable]
    public class CardUI
    {
        public string cardType;           // Nombre de la tarjeta
        public RawImage rawImage;         // Imagen de la tarjeta (RawImage)
        public TMP_Text textMeshPro;      // Texto de la tarjeta (TextMeshPro)
    }

    [Header("Tarjetas Configurables")]
    public List<CardUI> cardUIs; // Lista de configuraciones de tarjetas

    [Header("Colores")]
    public Color activeColor = Color.white;       // Color para imágenes activas
    public Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 1f); // Gris para inactivas
    public Color activeTextColor = Color.white;   // Color para textos activos
    public Color inactiveTextColor = Color.gray; // Color para textos inactivos

    private void Start()
    {
        UpdateUI(); // Inicializa la interfaz al empezar
    }

    private void Update()
    {
        UpdateUI(); // Actualiza en tiempo real
    }

    private void UpdateUI()
    {
        foreach (var cardUI in cardUIs)
        {
            // Verifica si el jugador tiene la tarjeta
            bool hasCard = InventorySystem.instance.HasCard(cardUI.cardType);

            // Cambia el color de la RawImage según el estado
            cardUI.rawImage.color = hasCard ? activeColor : inactiveColor;

            // Cambia el color del texto según el estado
            cardUI.textMeshPro.color = hasCard ? activeTextColor : inactiveTextColor;
        }
    }
}