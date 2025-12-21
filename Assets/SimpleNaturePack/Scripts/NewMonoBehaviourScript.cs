using UnityEngine;

public class Positivo : MonoBehaviour
{
    [Header("Partículas (>= 3)")]
    public ParticleSystem positiveParticles;

    [Header("Cielo (>= 7)")]
    public Material skyboxPinkBlue;

    [Header("Estrellas (>= 10)")]
    public GameObject starsObject;

    private Material defaultSkybox;

    private void Start()
    {
        defaultSkybox = RenderSettings.skybox;

        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged += HandleWorldState;
            HandleWorldState(WorldStateManager.Instance.worldState);
        }
    }

    private void OnDestroy()
    {
        if (WorldStateManager.Instance != null)
            WorldStateManager.Instance.OnWorldStateChanged -= HandleWorldState;
    }

    private void HandleWorldState(float state)
    {
        // 🔹 PARTÍCULAS
        if (positiveParticles != null)
        {
            if (state >= 3f)
                positiveParticles.Play();
            else
                positiveParticles.Stop();
        }

        // 🔹 SKYBOX
        if (skyboxPinkBlue != null)
        {
            if (state >= 7f)
                RenderSettings.skybox = skyboxPinkBlue;
            else
                RenderSettings.skybox = defaultSkybox;
        }

        // 🔹 ESTRELLAS
        if (starsObject != null)
        {
            starsObject.SetActive(state >= 10f);
        }
    }
}