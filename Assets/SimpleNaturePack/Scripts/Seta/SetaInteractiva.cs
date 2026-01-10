using UnityEngine;

public class SetaInteractiva : MonoBehaviour
{
    [Header("Configuración de la Seta")]
    public float effectValue = 1f; // +1 azul, -1 roja

    [Header("Feedback Visual")]
    public GameObject pickupEffect;

    private bool jugadorCerca = false;
    private Animator anim;
    private bool yaComida = false;

    [Header("UI")]
    public GameObject textoInteractuar;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaComida) // Añadimos !yaComida
        {
            jugadorCerca = true;
            anim = other.GetComponent<Animator>();

            if (textoInteractuar != null)
                textoInteractuar.SetActive(true);  //para el texto 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;

            if (textoInteractuar != null)
                textoInteractuar.SetActive(false); // para el texto
        }
    }
    private void OnDisable()
    {
        if (textoInteractuar != null)
            textoInteractuar.SetActive(false);
    }

    void Update()
    {
        // Añadimos !yaComida para que no  spamear la E
        if (jugadorCerca && !yaComida && Input.GetKeyDown(KeyCode.E))
        {
            yaComida = true; // Bloqueamos la seta para que no se coma 2 veces

            if (textoInteractuar != null)
                textoInteractuar.SetActive(false);// para el texto

            // --- EFECTO GLOBAL ---
            if (WorldStateManager.Instance != null)
            {
                WorldStateManager.Instance.ModifyWorldState(effectValue);
            }

            // --- EFECTO LOCAL ---
            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            // ANIMACION
            if (anim != null)
            {
                anim.SetTrigger("eat");
            }

            // Destruir con retraso para que se vea la animacion
            Destroy(gameObject, 2.5f);
        }
    }
}

