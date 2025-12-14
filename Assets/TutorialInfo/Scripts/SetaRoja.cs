using UnityEngine;

public class MushroomNegative : MonoBehaviour
{
    public int valueToRemove = 1;

    public void Consume()
    {
        WorldManager.Instance.AddWorldValue(-valueToRemove);
        Destroy(gameObject);
    }
}
