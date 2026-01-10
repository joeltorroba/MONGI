using UnityEngine;

public class GroundWorldReaction : MonoBehaviour
{
    [Header("Material del Suelo")]
    public Renderer groundRenderer;

    private void Start()
    {
        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged += OnWorldStateChanged;
        }
    }

    private void OnDestroy()
    {
        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged -= OnWorldStateChanged;
        }
    }

    void OnWorldStateChanged(float worldValue)
    {
        Debug.Log("El suelo recibe nuevo estado: " + worldValue);
    }
}