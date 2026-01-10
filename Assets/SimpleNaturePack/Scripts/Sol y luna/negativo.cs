using UnityEngine;

public class NegativeWorldEffects : MonoBehaviour
{
    [Header("Partículas distópicas (<= -3)")]
    public ParticleSystem negativeParticles;

    [Header("Cielo distópico (<= -7)")]
    public Material skyboxGrayRed;

    [Header("Estrellas apagadas (<= -10)")]
    public GameObject starsObject;

    [Header("Iluminación Distopía")]
    public Light directionalLight;
    public float dystopiaLightIntensity = 0.5f;
    public float dystopiaAmbientIntensity = 0.2f;
    public Color dystopiaAmbientColor = new Color(0.2f, 0.1f, 0.1f);

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
        if (negativeParticles != null)
        {
            if (state <= -3f && !negativeParticles.isPlaying)
                negativeParticles.Play();
            else if (state > -3f && negativeParticles.isPlaying)
                negativeParticles.Stop();
        }

        if (state <= -7f)
        {
            if (skyboxGrayRed != null)
                RenderSettings.skybox = skyboxGrayRed;

            RenderSettings.ambientIntensity = dystopiaAmbientIntensity;
            RenderSettings.ambientLight = dystopiaAmbientColor;

            if (directionalLight != null)
                directionalLight.intensity = dystopiaLightIntensity;
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
            starsObject.SetActive(state <= -10f);

        DynamicGI.UpdateEnvironment();
    }
}
