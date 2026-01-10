using UnityEngine;

public class NieblaReactiva : MonoBehaviour
{
    [Header("Mundo Neutro (Estado 0)")]
    public Color colorNeutro = Color.gray;   
    public float densidadNeutra = 0.02f;     

    [Header("Mundo ROJO / Distopía (Hacia -10)")]
    public Color colorRojo = new Color(0.3f, 0, 0); 
    public float densidadRoja = 0.08f;              

    [Header("Mundo AZUL / Utopía (Hacia +10)")]
    public Color colorAzul = new Color(0.8f, 0.9f, 1f); 
    public float densidadAzul = 0.005f;                 

    void Start()
    {
        RenderSettings.fog = true;

        RenderSettings.fogMode = FogMode.Exponential;

        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged += ActualizarNiebla;

            ActualizarNiebla(WorldStateManager.Instance.worldState);
        }
    }

    void OnDestroy()
    {
        if (WorldStateManager.Instance != null)
            WorldStateManager.Instance.OnWorldStateChanged -= ActualizarNiebla;
    }

    void ActualizarNiebla(float estado)
    {
        if (estado > 0)
        {
            float t = estado / 10f;
            RenderSettings.fogColor = Color.Lerp(colorNeutro, colorAzul, t);
            RenderSettings.fogDensity = Mathf.Lerp(densidadNeutra, densidadAzul, t);
        }

        else
        {
            float t = Mathf.Abs(estado) / 10f;
            RenderSettings.fogColor = Color.Lerp(colorNeutro, colorRojo, t);
            RenderSettings.fogDensity = Mathf.Lerp(densidadNeutra, densidadRoja, t);
        }
    }
}