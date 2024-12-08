using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TimedPanelController : MonoBehaviour
{
    [Header("Configuraciones del Panel")]
    public GameObject panel; // Asigna aquí el panel en el Inspector
    public Button closeButton; // Asigna el botón para cerrar el panel

    [Header("Tiempo de Espera")]
    public float waitTime = 30f; // Tiempo de espera en segundos

    private void Start()
    {
        // Asegúrate de que el panel esté inicialmente desactivado
        if (panel != null)
            panel.SetActive(false);

        // Inicia la espera
        StartCoroutine(ShowPanelAfterDelay());
    }

    private IEnumerator ShowPanelAfterDelay()
    {
        // Espera el tiempo configurado
        yield return new WaitForSeconds(waitTime);

        // Muestra el panel
        if (panel != null)
            panel.SetActive(true);

        // Asocia el evento de cierre al botón
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);
    }

    private void ClosePanel()
    {
        // Cierra el panel al presionar el botón
        if (panel != null)
            panel.SetActive(false);
    }
}