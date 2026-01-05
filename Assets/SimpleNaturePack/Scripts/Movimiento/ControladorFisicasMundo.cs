using UnityEngine;

public class ControladorFisicasMundo : MonoBehaviour
{
    [Header("Referencias")]
    public MovimientoPers scriptDeMovimiento;
    public Animator animadorJugador;

    [Header("Configuración UTOPÍA")]
    public float velocidadRapida = 4.0f; 
    public float velocidadAnimacionRapida = 1.5f;

    [Header("Configuración DISTOPÍA")]
    public float velocidadLenta = 1.0f;  
    public float velocidadAnimacionLenta = 0.5f;

    private float velocidadOriginal;

    void Start()
    {
        if (scriptDeMovimiento != null)
        {
            velocidadOriginal = scriptDeMovimiento.velocidadMovimiento;
        }

        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged += CambiarFisicasGradual;

            CambiarFisicasGradual(WorldStateManager.Instance.worldState);
        }
    }

    void OnDestroy()
    {
        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged -= CambiarFisicasGradual;
        }
    }

    void CambiarFisicasGradual(float estado)
    {
        if (scriptDeMovimiento == null) return;

        float porcentaje = Mathf.Abs(estado) / 10f;

        //CASO UTOPÍA 
        if (estado > 0)
        {
            scriptDeMovimiento.velocidadMovimiento = Mathf.Lerp(velocidadOriginal, velocidadRapida, porcentaje);
            if (animadorJugador != null)
                animadorJugador.speed = Mathf.Lerp(1f, velocidadAnimacionRapida, porcentaje);
        }

        //CASO DISTOPÍA
        else
        {
            scriptDeMovimiento.velocidadMovimiento = Mathf.Lerp(velocidadOriginal, velocidadLenta, porcentaje);
            if (animadorJugador != null)
                animadorJugador.speed = Mathf.Lerp(1f, velocidadAnimacionLenta, porcentaje);
        }
    }
}