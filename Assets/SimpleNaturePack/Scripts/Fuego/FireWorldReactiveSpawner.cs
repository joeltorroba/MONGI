using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorldReactiveSpawner : MonoBehaviour
{
    [Header("Asigna el manager si quieres (si no, lo busca solo)")]
    public WorldStateManager world;

    [Header("Prefab y puntos de spawn")]
    public GameObject firePrefab;
    public Transform[] spots;

    [Header("Distopía (negativo)")]
    public float startAtWorldState = -2f;
    public float fullAtWorldState = -10f;
    public int maxFires = 15;

    [Header("Gradual")]
    public float spawnInterval = 0.25f;
    public float removeInterval = 0.25f;

    private GameObject[] spawned;
    private bool[] used;
    private int currentFires = 0;

    private Coroutine spawnRoutine;
    private Coroutine removeRoutine;

    void Awake()
    {
        spawned = new GameObject[spots.Length];
        used = new bool[spots.Length];
    }

    void OnEnable()
    {
        TrySubscribe(); // por si ya existe el manager
    }

    void Start()
    {
        TrySubscribe(); // <-- CLAVE: aquí casi siempre ya existe el Instance
        if (world != null)
            OnWorldStateChanged(world.worldState); // sincroniza al arrancar
    }

    void OnDisable()
    {
        if (world != null)
            world.OnWorldStateChanged -= OnWorldStateChanged;
    }

    void TrySubscribe()
    {
        if (world == null) world = WorldStateManager.Instance;
        if (world == null) world = FindFirstObjectByType<WorldStateManager>();

        if (world == null)
        {
            StartCoroutine(RetrySubscribeNextFrame());
            return;
        }

        // evita doble suscripción
        world.OnWorldStateChanged -= OnWorldStateChanged;
        world.OnWorldStateChanged += OnWorldStateChanged;

        
    }
    private IEnumerator RetrySubscribeNextFrame()
    {
        yield return null; // espera 1 frame
        TrySubscribe();
    }

    void OnWorldStateChanged(float ws)
    {
        int target = CalculateTargetFires(ws);
        

        if (target > currentFires)
        {
            if (removeRoutine != null) StopCoroutine(removeRoutine);
            if (spawnRoutine != null) StopCoroutine(spawnRoutine);
            spawnRoutine = StartCoroutine(SpawnUntil(target));
        }
        else if (target < currentFires)
        {
            if (spawnRoutine != null) StopCoroutine(spawnRoutine);
            if (removeRoutine != null) StopCoroutine(removeRoutine);
            removeRoutine = StartCoroutine(RemoveUntil(target));
        }
    }

    int CalculateTargetFires(float ws)
    {
        float t = Mathf.InverseLerp(startAtWorldState, fullAtWorldState, ws);
        t = Mathf.Clamp01(t);

        int limit = Mathf.Min(maxFires, spots.Length);
        return Mathf.RoundToInt(t * limit);
    }

    IEnumerator SpawnUntil(int target)
    {
        while (currentFires < target)
        {
            SpawnOne();
            yield return new WaitForSeconds(spawnInterval);
        }
        spawnRoutine = null;
    }

    void SpawnOne()
    {
        int idx = GetRandomUnusedSpot();
        if (idx == -1) return;

        var fire = Instantiate(firePrefab, spots[idx].position, spots[idx].rotation);
        spawned[idx] = fire;
        used[idx] = true;
        currentFires++;

        var fader = fire.GetComponent<FireFader>();
        if (fader != null) fader.PlayFadeIn();
    }

    IEnumerator RemoveUntil(int target)
    {
        while (currentFires > target)
        {
            RemoveOne();
            yield return new WaitForSeconds(removeInterval);
        }
        removeRoutine = null;
    }

    void RemoveOne()
    {
        int idx = GetRandomUsedSpot();
        if (idx == -1) return;

        var fire = spawned[idx];
        spawned[idx] = null;
        used[idx] = false;
        currentFires--;

        if (fire != null)
        {
            var fader = fire.GetComponent<FireFader>();
            if (fader != null) fader.PlayFadeOutAndDestroy();
            else Destroy(fire);
        }
    }

    int GetRandomUnusedSpot()
    {
        List<int> c = new List<int>();
        for (int i = 0; i < used.Length; i++) if (!used[i]) c.Add(i);
        return c.Count == 0 ? -1 : c[Random.Range(0, c.Count)];
    }

    int GetRandomUsedSpot()
    {
        List<int> c = new List<int>();
        for (int i = 0; i < used.Length; i++) if (used[i]) c.Add(i);
        return c.Count == 0 ? -1 : c[Random.Range(0, c.Count)];
    }
}
