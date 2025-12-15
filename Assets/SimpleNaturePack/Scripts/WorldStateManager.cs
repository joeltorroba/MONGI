using UnityEngine;
using System;

public class WorldStateManager : MonoBehaviour
{
    public static WorldStateManager Instance;

    [Header("Estado Global del Mundo")]
    [Range(-10f, 10f)]
    public float worldState = 0f;

    public event Action<float> OnWorldStateChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ModifyWorldState(float value)
    {
        worldState += value;
        worldState = Mathf.Clamp(worldState, -10f, 10f);

        Debug.Log("Estado global del mundo: " + worldState);

        OnWorldStateChanged?.Invoke(worldState);
    }
}

