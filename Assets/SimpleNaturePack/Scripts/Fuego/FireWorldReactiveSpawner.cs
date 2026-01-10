using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorldReactiveSpawner : MonoBehaviour
{
    [Header("Prefab y puntos de spawn")]
    public GameObject firePrefab;
    public Transform[] spots;

    [Header("Regla de aparición (distopía)")]
    [Tooltip("A partir de qué worldState empieza a aparecer fuego (ej: 1 o 2).")]
    public float startAtWorldState = 2f;

    [Tooltip("Cuántos fuegos por cada 1 punto de worldState por encima del start.")]
    public int firesPerPoint = 1;

    [Tooltip("Máximo de fuegos activos.")]
    public int maxFires = 15;

    [Header("Aparición gradual")]
    public float spawnInterval = 0.25f;

    [Header("Opcional")]
    [Tooltip("Si está activo, el fuego solo crece (no se apaga si el mundo mejora).")]
    public bool onlyGrow = true;

    private GameObject[] spawned;
    private bool[] used;
    private int currentFires = 0;
    private Coroutine spawnRoutine;

    private void Awake()
    {
        spawned = new GameObject[spots.Length];
        used = new bool[spots.Length];
    }

    private void OnEnable()
    {
        if (WorldStateManager.Instance != null)
            WorldStateManager.Instance.OnWorldStateChanged += OnWorldStateChanged;
    }

    private void Start()
    {
        // Inicializa según el estado actual al arrancar
        if (WorldStateManager.Instance != null)
            OnWorldStateChanged(WorldStateManager.Instance.worldState);
    }

    private void OnDisable()
    {
        if (WorldStateManager.Instance != null)
            WorldStateManager.Instance.OnWorldStateChanged -= OnWorldStateChanged;
    }

    private void OnWorldStateChanged(float ws)
    {
        // Asumimos: distopía = ws positivo (seta mala suma)
        // Si en tu proyecto es al revés, te digo abajo cómo invertirlo.

        int target = CalculateTargetFires(ws);

        if (onlyGrow) target = Mathf.Max(target, currentFires);

        if (target > currentFires)
        {
            if (spawnRoutine != null) StopCoroutine(spawnRoutine);
            spawnRoutine = StartCoroutine(SpawnUntil(target));
        }
        else if (!onlyGrow && target < currentFires)
        {
            RemoveUntil(target);
        }
    }

    private int CalculateTargetFires(float ws)
    {
        if (ws <= startAtWorldState) return 0;

        float above = ws - startAtWorldState; // ej ws=3, start=2 => above=1
        int target = Mathf.FloorToInt(above * firesPerPoint);

        target = Mathf.Clamp(target, 0, Mathf.Min(maxFires, spots.Length));
        return target;
    }

    private IEnumerator SpawnUntil(int target)
    {
        while (currentFires < target)
        {
            SpawnOne();
            yield return new WaitForSeconds(spawnInterval);
        }
        spawnRoutine = null;
    }

    private void SpawnOne()
    {
        int idx = GetRandomUnusedSpot();
        if (idx == -1) return;

        GameObject fire = Instantiate(firePrefab, spots[idx].position, spots[idx].rotation);
        spawned[idx] = fire;
        used[idx] = true;
        currentFires++;
    }

    private int GetRandomUnusedSpot()
    {
        List<int> candidates = new List<int>();
        for (int i = 0; i < used.Length; i++)
            if (!used[i]) candidates.Add(i);

        if (candidates.Count == 0) return -1;
        return candidates[Random.Range(0, candidates.Count)];
    }

    private void RemoveUntil(int target)
    {
        for (int i = used.Length - 1; i >= 0 && currentFires > target; i--)
        {
            if (used[i])
            {
                if (spawned[i] != null) Destroy(spawned[i]);
                spawned[i] = null;
                used[i] = false;
                currentFires--;
            }
        }
    }
}

