using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform player;  // Aquí arrastras el personaje al que la cámara seguirá
    public Vector3 offset;    // Puedes ajustar la distancia entre la cámara y el personaje

    void LateUpdate()
    {
        // Actualiza la posición de la cámara para que siga al personaje
        transform.position = player.position + offset;
    }
}
