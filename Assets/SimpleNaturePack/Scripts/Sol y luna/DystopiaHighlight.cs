using UnityEngine;

public class DystopiaHighlight : MonoBehaviour
{
    public Material highlightMaterial;
    private Material[] originalMaterials;
    private Renderer rend;

    private void OnEnable()
    {
        rend = GetComponent<Renderer>();
        originalMaterials = rend.materials;

        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged += OnWorldStateChanged;
            OnWorldStateChanged(WorldStateManager.Instance.worldState);
        }
    }

    private void OnDisable()
    {
        if (WorldStateManager.Instance != null)
            WorldStateManager.Instance.OnWorldStateChanged -= OnWorldStateChanged;
    }

    void OnWorldStateChanged(float state)
    {
        if (state <= -8f)
        {
            Material[] mats = new Material[originalMaterials.Length + 1];
            originalMaterials.CopyTo(mats, 0);
            mats[mats.Length - 1] = highlightMaterial;
            rend.materials = mats;
        }
        else
        {
            rend.materials = originalMaterials;
        }
    }
}
