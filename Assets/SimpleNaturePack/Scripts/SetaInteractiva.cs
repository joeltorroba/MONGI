using UnityEngine;

public class SetaInteractiva : MonoBehaviour
{
    private bool jugadorCerca = false;
    private Animator anim;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            anim = other.GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("eat");
            Destroy(gameObject, 3.5f);
        }
    }
}
