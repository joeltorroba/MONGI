using UnityEngine;

public class ArbolFlotante : MonoBehaviour
{
    private Vector3 posicionInicial; // Guardamos donde estaba el árbol al principio

    [Header("Configuración")]
    [Tooltip("Cuántos metros subirá el árbol si llegas al +10 (Utopía Máxima)")]
    public float alturaMaxima = 3.0f;

    void Start()
    {
        // 1. Memorizamos la posición original (en el suelo)
        posicionInicial = transform.position;

        // 2. Nos suscribimos al WorldManager
        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged += ActualizarAltura;

            // Actualizamos ya por si empezamos con puntos
            ActualizarAltura(WorldStateManager.Instance.worldState);
        }
    }

    void OnDestroy()
    {
        // Nos desuscribimos para evitar errores
        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged -= ActualizarAltura;
        }
    }

    void ActualizarAltura(float estado)
    {
        // Solo flotamos si estamos en Utopía (valores positivos)
        if (estado > 0)
        {
            // Calculamos el porcentaje (0.1, 0.5, 1.0...)
            float porcentaje = estado / 10f;

            // Calculamos la nueva altura
            // Nueva Y = Y original + (5 metros * porcentaje)
            Vector3 nuevaPosicion = posicionInicial;
            nuevaPosicion.y += alturaMaxima * porcentaje;

            transform.position = nuevaPosicion;
        }
        else
        {
            // Si estamos en 0 o en Distopía (-), volvemos al suelo original
            transform.position = posicionInicial;
        }
    }
}