using UnityEngine;

public class WorldStateObjectController : MonoBehaviour
{
    [Header("Objetos según estado del mundo")]
    public GameObject objectWhenNegative;
    public GameObject objectWhenPositive;

    private void Start()
    {
        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged += HandleWorldState;
            HandleWorldState(WorldStateManager.Instance.worldState);
        }
    }

    private void OnDestroy()
    {
        if (WorldStateManager.Instance != null)
            WorldStateManager.Instance.OnWorldStateChanged -= HandleWorldState;
    }

    private void HandleWorldState(float state)
    {
        if (objectWhenNegative != null)
            objectWhenNegative.SetActive(state <= -1f);

        if (objectWhenPositive != null)
            objectWhenPositive.SetActive(state >= 1f);
    }
}
