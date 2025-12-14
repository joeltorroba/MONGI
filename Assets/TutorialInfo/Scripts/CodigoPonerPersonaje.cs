using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject currentMushroom;

    private void Update()
    {
        if (currentMushroom != null && Input.GetKeyDown(KeyCode.E))
        {
            if (currentMushroom.TryGetComponent(out MushroomPositive positive))
            {
                positive.Consume();
            }
            else if (currentMushroom.TryGetComponent(out MushroomNegative negative))
            {
                negative.Consume();
            }

            currentMushroom = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mushroom"))
        {
            currentMushroom = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentMushroom)
        {
            currentMushroom = null;
        }
    }
}
