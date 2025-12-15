using System;
using UnityEngine;

public enum WorldState
{
    Dystopic = -1,
    Neutral = 0,
    Utopic = 1
}

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    public WorldState currentState = WorldState.Neutral;

    public event Action<WorldState> OnWorldStateChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetWorldState(WorldState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        OnWorldStateChanged?.Invoke(currentState);
    }
}
