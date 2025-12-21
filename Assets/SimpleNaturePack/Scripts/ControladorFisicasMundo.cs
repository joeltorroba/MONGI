using UnityEngine;

public class ControladorFisicasMundo : MonoBehaviour
{
    public MovimientoPers scriptDeMovimiento; 
    public Animator animadorJugador;      

    [Header("Configuración UTOPÍA")]
    public float velocidadRapida = 6.0f; 
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
            WorldStateManager.Instance.OnWorldStateChanged += CambiarFisicas;
            CambiarFisicas(WorldStateManager.Instance.worldState);
        }
    }

    
    void OnDestroy()
    {
        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged -= CambiarFisicas;
        }
    }

    void CambiarFisicas(float estado)
    {
        
        if (scriptDeMovimiento == null) return;

        if (estado <= -3f)
        {
            scriptDeMovimiento.velocidadMovimiento = velocidadLenta;
            if (animadorJugador != null) animadorJugador.speed = velocidadAnimacionLenta;
        }


        else if (estado >= 3f)
        {
            scriptDeMovimiento.velocidadMovimiento = velocidadRapida;
            if (animadorJugador != null) animadorJugador.speed = velocidadAnimacionRapida;
        }

        else
        {
            scriptDeMovimiento.velocidadMovimiento = velocidadOriginal;
            if (animadorJugador != null) animadorJugador.speed = 1f;
        }
    }
}