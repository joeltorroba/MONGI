using UnityEngine;

public class PositiveWorldAudio : MonoBehaviour
{
    public AudioSource birdsAudio;
    public float maxWorldValue = 10f;
    public float maxVolume = 1f;

    private void Start()
    {
        if (birdsAudio == null) birdsAudio = GetComponent<AudioSource>();
        if (birdsAudio != null)
        {
            birdsAudio.loop = true;
            birdsAudio.Play();
            birdsAudio.volume = 0f;
        }
    }

    private void Update()
    {

        if (WorldStateManager.Instance != null && birdsAudio != null)
        {
            float estadoActual = WorldStateManager.Instance.worldState;


            if (estadoActual > 0)
            {
                float volume = (estadoActual / maxWorldValue) * maxVolume;
                birdsAudio.volume = Mathf.Clamp01(volume);


                if (!birdsAudio.isPlaying) birdsAudio.Play();
            }
            else
            {

                birdsAudio.volume = 0f;
            }
        }
    }
}
