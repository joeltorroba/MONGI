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
        // El "vigilante": mira el estado 60 veces por segundo
        if (WorldStateManager.Instance != null && birdsAudio != null)
        {
            float estadoActual = WorldStateManager.Instance.worldState;

            // Si el mundo es positivo (Azul)
            if (estadoActual > 0)
            {
                float volume = (estadoActual / maxWorldValue) * maxVolume;
                birdsAudio.volume = Mathf.Clamp01(volume);

                // Si por algún error de Unity se para, lo despertamos
                if (!birdsAudio.isPlaying) birdsAudio.Play();
            }
            else
            {
                // Si es 0 o negativo, los pájaros se callan
                birdsAudio.volume = 0f;
            }
        }
    }
}
