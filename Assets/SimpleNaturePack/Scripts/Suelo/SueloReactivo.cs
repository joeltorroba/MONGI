using UnityEngine;

public class SueloReactivo : MonoBehaviour
{
    private Renderer miRenderer;

    [Header("Configuración de Colores")]
    public Color colorDistopia = Color.red;    // -10
    public Color colorNeutro = Color.white;    // 0 (El color normal del suelo)
    public Color colorUtopia = Color.blue;     // +10

    void Start()
    {
        miRenderer = GetComponent<Renderer>();

        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged += ActualizarColorSuelo;

            // Iniciamos con el color que toque
            ActualizarColorSuelo(WorldStateManager.Instance.worldState);
        }
    }

    void OnDestroy()
    {
        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged -= ActualizarColorSuelo;
        }
    }

    void ActualizarColorSuelo(float estado)
    {
        // CASO 1: Estamos en el lado positivo (UTOPÍA)
        if (estado > 0)
        {
            // Calculamos el porcentaje de 0 a 10.
            // Si estado es 5, 't' será 0.5 (50% azul, 50% neutro)
            float t = estado / 10f;

            // Mezclamos gradual entre Neutro y Azul
            miRenderer.material.color = Color.Lerp(colorNeutro, colorUtopia, t);
        }
        // CASO 2: Estamos en el lado negativo (DISTOPÍA)
        else
        {
            // Usamos valor absoluto (Abs) para quitar el signo negativo
            // Si estado es -5, queremos un 0.5 (50% rojo)
            float t = Mathf.Abs(estado) / 10f;

            // Mezclamos gradual entre Neutro y Rojo
            miRenderer.material.color = Color.Lerp(colorNeutro, colorDistopia, t);
        }
    }
}