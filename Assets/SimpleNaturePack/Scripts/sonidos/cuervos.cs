using UnityEngine;

public class NegativeWorldAudio : MonoBehaviour
{
    public AudioSource crowsAudio;

    [Header("Escalado directo")]
    public float minWorldValue = -10f; 
    public float maxVolume = 1f;       

    private void Start()
    {
        if (crowsAudio == null)
            return;

        crowsAudio.loop = true;
        crowsAudio.playOnAwake = false;
        crowsAudio.volume = 0f;
        crowsAudio.Play();
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

        if (state >= 0f)
        {
            crowsAudio.volume = 0f;
            return;
        }


        float volume = Mathf.Abs(state / minWorldValue) * maxVolume;

        crowsAudio.volume = Mathf.Clamp01(volume);
    }
}
