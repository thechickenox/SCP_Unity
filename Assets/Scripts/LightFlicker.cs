using UnityEngine;
using UnityEngine.Rendering.Universal; // Importar para usar Light2D

public class URPLightFlicker : MonoBehaviour
{
    public Light2D flickerLight;          // Luz 2D que parpadeará
    public float minIntensity = 0.5f;     // Intensidad mínima de la luz
    public float maxIntensity = 1.5f;     // Intensidad máxima de la luz
    public float flickerSpeed = 0.1f;     // Velocidad del parpadeo

    private float targetIntensity;
    private float timer;

    void Start()
    {
        // Configurar la luz 2D desde el componente adjunto
        if (flickerLight == null)
            flickerLight = GetComponent<Light2D>();

        targetIntensity = flickerLight.intensity;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            // Generar una nueva intensidad aleatoria dentro del rango
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            timer = flickerSpeed;
        }

        // Interpolación para suavizar el cambio de intensidad
        flickerLight.intensity = Mathf.Lerp(flickerLight.intensity, targetIntensity, Time.deltaTime * 10f);
    }
}