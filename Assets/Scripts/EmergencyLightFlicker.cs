using UnityEngine;
using UnityEngine.Rendering.Universal; // Importar para usar Light2D

public class EmergencyLightFlicker : MonoBehaviour
{
    public Light2D flickerLight;          // Luz 2D que parpadeará
    public float lowIntensity = 0.0f;     // Intensidad baja (luz apagada)
    public float highIntensity = 1.5f;    // Intensidad alta (luz encendida)
    public float flickerInterval = 0.5f;  // Intervalo de parpadeo en segundos

    private float timer;
    private bool isHighIntensity;         // Bandera para alternar entre intensidades

    void Start()
    {
        // Configurar la luz 2D desde el componente adjunto
        if (flickerLight == null)
            flickerLight = GetComponent<Light2D>();

        timer = flickerInterval;          // Establecer el tiempo inicial
        isHighIntensity = true;           // Comenzar con la intensidad alta
        flickerLight.intensity = highIntensity;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            // Alternar la intensidad entre alta y baja
            isHighIntensity = !isHighIntensity;
            flickerLight.intensity = isHighIntensity ? highIntensity : lowIntensity;
            timer = flickerInterval;      // Reiniciar el temporizador
        }
    }
}