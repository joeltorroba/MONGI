using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    [Range(0, 10)]
    public int worldValue = 5;

    private const int minValue = 0;
    private const int maxValue = 10;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddWorldValue(int amount)
    {
        worldValue += amount;
        worldValue = Mathf.Clamp(worldValue, minValue, maxValue);

        Debug.Log("World Value: " + worldValue);
        UpdateWorld();
    }

    private void UpdateWorld()
    {

        if (worldValue >= 7)
        {
            Debug.Log("UTOPIA");
        }
        else if (worldValue <= 3)
        {
            Debug.Log("DISTOPIA");
        }
        else
        {
            Debug.Log("MUNDO NEUTRO");
        }
    }
}

