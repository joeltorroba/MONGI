using UnityEngine;

public class CameraShakeReactivo : MonoBehaviour
{
    private Vector3 posicionOriginal;

    [Header("Configuración Micro-vibración")]
    public float intensidadMaxima = 0.008f; 
    public float rapidez = 20f;

    void Start()
    {
        posicionOriginal = transform.localPosition;
    }

    void Update()
    {
        if (WorldStateManager.Instance != null)
        {
            float estado = WorldStateManager.Instance.worldState;

            if (estado < 0)
            {
                float factor = Mathf.Abs(estado) / 10f;

                float balanceoX = (Mathf.PerlinNoise(Time.time * rapidez, 0f) - 0.5f) * 2f;
                float balanceoY = (Mathf.PerlinNoise(0f, Time.time * rapidez) - 0.5f) * 2f;

                Vector3 movimiento = new Vector3(balanceoX, balanceoY, 0) * intensidadMaxima * factor;

                transform.localPosition = posicionOriginal + movimiento;
            }
            else
            {
                
                transform.localPosition = Vector3.Lerp(transform.localPosition, posicionOriginal, Time.deltaTime * 2f);
            }
        }
    }
}