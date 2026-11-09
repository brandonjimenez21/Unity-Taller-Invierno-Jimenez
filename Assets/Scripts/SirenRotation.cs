using UnityEngine;

/// <summary>
/// Hace girar las luces de emergencia de la patrulla.
/// Se asigna unicamente a Siren_Pivot: al rotar el pivote, sus hijos
/// Red_Light y Blue_Light giran en circulo alrededor del techo.
/// </summary>
public class SirenRotation : MonoBehaviour
{
    [Tooltip("Velocidad de giro en grados por segundo (ajustable desde el Inspector).")]
    public float rotationSpeed = 360f;

    void Update()
    {
        // Time.deltaTime hace que la velocidad sea igual sin importar los FPS.
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
