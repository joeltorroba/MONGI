using UnityEngine;

public class PositiveWorldEffects : MonoBehaviour
{
    [Header("Partículas (>= 3)")]
    public ParticleSystem positiveParticles;

    [Header("Cielo (>= 7)")]
    public Material skyboxPinkBlue;

    [Header("Estrellas (>= 10)")]
    public GameObject starsObject;

    [Header("Iluminación Utopía")]
    public Light directionalLight;
    public float utopiaLightIntensity = 4f;
    public float utopiaAmbientIntensity = 4f;
    public Color utopiaAmbientColor = new Color(0.8f, 0.7f, 0.9f);

    private Material defaultSkybox;
    private float defaultLightIntensity;
    private float defaultAmbientIntensity;
    private Color defaultAmbientColor;

    private void OnEnable()
    {
        defaultSkybox = RenderSettings.skybox;

        if (directionalLight != null)
            defaultLightIntensity = directionalLight.intensity;

        defaultAmbientIntensity = RenderSettings.ambientIntensity;
        defaultAmbientColor = RenderSettings.ambientLight;

        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged += HandleWorldState;
            HandleWorldState(WorldStateManager.Instance.worldState);
        }
    }

    private void OnDisable()
    {
        if (WorldStateManager.Instance != null)
            WorldStateManager.Instance.OnWorldStateChanged -= HandleWorldState;
    }

    private void HandleWorldState(float state)
    {

        if (positiveParticles != null)
        {
            if (state >= 3f && !positiveParticles.isPlaying)
                positiveParticles.Play();
            else if (state < 3f && positiveParticles.isPlaying)
                positiveParticles.Stop();
        }


        if (state >= 7f)
        {
            if (skyboxPinkBlue != null)
                RenderSettings.skybox = skyboxPinkBlue;

            RenderSettings.ambientIntensity = utopiaAmbientIntensity;
            RenderSettings.ambientLight = utopiaAmbientColor;

            if (directionalLight != null)
                directionalLight.intensity = utopiaLightIntensity;
        }
        else
        {
            RenderSettings.skybox = defaultSkybox;
            RenderSettings.ambientIntensity = defaultAmbientIntensity;
            RenderSettings.ambientLight = defaultAmbientColor;

            if (directionalLight != null)
                directionalLight.intensity = defaultLightIntensity;
        }


        if (starsObject != null)
            starsObject.SetActive(state >= 10f);


        DynamicGI.UpdateEnvironment();
    }
}
