using UnityEngine;

public class ReactiveFOV : MonoBehaviour
{
    private Camera cam;

    [Header("Configuración del FOV")]
    [Tooltip("Valor para el estado -10 (Distopía). Sugerencia: 40-50")]
    public float fovDistopia = 50f;

    [Tooltip("Valor para el estado 0 (Neutro). Sugerencia: 60")]
    public float fovNeutro = 60f;

    [Tooltip("Valor para el estado +10 (Utopía). Sugerencia: 80-90")]
    public float fovUtopia = 90f;

    void Start()
    {
        cam = GetComponent<Camera>();

        // Suscripción al WorldManager
        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged += ActualizarFOV;

            // Inicializar al arrancar
            ActualizarFOV(WorldStateManager.Instance.worldState);
        }
    }

    void OnDestroy()
    {
        // Desuscripción obligatoria
        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged -= ActualizarFOV;
        }
    }

    void ActualizarFOV(float estadoActual)
    {
        // CASO 1: Utopía (Positivo 0 a 10) -> Abrimos ángulo
        if (estadoActual > 0)
        {
            float t = estadoActual / 10f; // Porcentaje (0 a 1)
            cam.fieldOfView = Mathf.Lerp(fovNeutro, fovUtopia, t);
        }
        // CASO 2: Distopía (Negativo 0 a -10) -> Cerramos ángulo
        else
        {
            float t = Mathf.Abs(estadoActual) / 10f; // Porcentaje (0 a 1) sin signo
            cam.fieldOfView = Mathf.Lerp(fovNeutro, fovDistopia, t);
        }
    }
}