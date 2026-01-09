using UnityEngine;

public class PositiveWorldAudio : MonoBehaviour
{
    public AudioSource birdsAudio;

    [Header("Escalado directo")]
    public float maxWorldValue = 10f; 
    public float maxVolume = 1f;      

    private void Start()
    {
        if (birdsAudio == null)
            return;

        birdsAudio.loop = true;
        birdsAudio.playOnAwake = false;
        birdsAudio.volume = 0f;
        birdsAudio.Play();
    }

    private void OnEnable()
    {
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
        // Solo reaccionamos a valores positivos
        if (state <= 0f)
        {
            birdsAudio.volume = 0f;
            return;
        }

        float volume = (state / maxWorldValue) * maxVolume;

        birdsAudio.volume = Mathf.Clamp01(volume);
    }
}
