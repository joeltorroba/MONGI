using System.Collections;
using UnityEngine;

public class FireFader : MonoBehaviour
{
    [Header("Refs (opcionales, si los dejas null se auto-buscan)")]
    public ParticleSystem ps;
    public Light fireLight;

    [Header("Fade")]
    public float fadeInTime = 0.6f;
    public float fadeOutTime = 0.8f;

    [Tooltip("Rate objetivo al encender (ajústalo a tu gusto).")]
    public float targetEmissionRate = 40f;

    [Tooltip("Intensidad objetivo de la luz (si hay).")]
    public float targetLightIntensity = 2f;

    [Tooltip("Tiempo extra antes de destruir tras apagar (para que mueran partículas).")]
    public float destroyDelay = 1.2f;

    private Coroutine routine;

    void Awake()
    {
        if (ps == null) ps = GetComponentInChildren<ParticleSystem>();
        if (fireLight == null) fireLight = GetComponentInChildren<Light>();
    }

    public void PlayFadeIn()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FadeTo(targetEmissionRate, targetLightIntensity, fadeInTime, play: true));
    }

    public void PlayFadeOutAndDestroy()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FadeOutDestroy());
    }

    private IEnumerator FadeOutDestroy()
    {
        // baja emisión y luz a 0
        yield return FadeTo(0f, 0f, fadeOutTime, play: true);

        // deja de emitir para que no nazcan nuevas
        if (ps != null)
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    private IEnumerator FadeTo(float emissionRate, float lightIntensity, float time, bool play)
    {
        if (ps == null) yield break;

        if (play && !ps.isPlaying) ps.Play(true);

        var em = ps.emission;

        float startRate = 0f;
        // leer el valor actual si existe
        try { startRate = em.rateOverTime.constant; } catch { startRate = 0f; }

        float startLight = fireLight != null ? fireLight.intensity : 0f;

        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / time);

            float r = Mathf.Lerp(startRate, emissionRate, k);
            em.rateOverTime = r;

            if (fireLight != null)
                fireLight.intensity = Mathf.Lerp(startLight, lightIntensity, k);

            yield return null;
        }

        em.rateOverTime = emissionRate;
        if (fireLight != null) fireLight.intensity = lightIntensity;
    }
}
