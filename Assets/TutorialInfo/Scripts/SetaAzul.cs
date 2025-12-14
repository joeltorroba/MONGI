using UnityEngine;

public class MushroomPositive : MonoBehaviour
{
    public int valueToAdd = 1;

    public void Consume()
    {
        WorldManager.Instance.AddWorldValue(valueToAdd);
        Destroy(gameObject);
    }
}
