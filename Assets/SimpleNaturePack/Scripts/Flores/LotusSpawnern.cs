using UnityEngine;
using System.Collections.Generic;

public class LotusSpawnern : MonoBehaviour
{
    [Header("Prefab de la flor")]
    public GameObject lotusPrefab;

    [Header("Altura sobre el Bush_03")]
    public float heightOffset = 1f;

    private GameObject[] bushes;
    private int currentSpawnLevel = 0;

    // Lista interna para guardar las flores creadas
    private List<GameObject> spawnedLotus = new List<GameObject>();

    private void Start()
    {
        bushes = GameObject.FindGameObjectsWithTag("Bush_03");
        WorldStateManager.Instance.OnWorldStateChanged += HandleWorldStateChanged;
    }

    private void OnDestroy()
    {
        WorldStateManager.Instance.OnWorldStateChanged -= HandleWorldStateChanged;
    }

    private void HandleWorldStateChanged(float newState)
    {
        int spawnLevel = 0;

        if (newState <= -9)
            spawnLevel = 1;
        else
            spawnLevel = 0; //Aquí deben desaparecer

        if (spawnLevel == currentSpawnLevel)
            return;

        currentSpawnLevel = spawnLevel;

        SpawnLotus(spawnLevel);
    }

    private void SpawnLotus(int amountPerBush)
    {
        //Eliminar TODAS las flores anteriores
        foreach (var lotus in spawnedLotus)
        {
            if (lotus != null)
                Destroy(lotus);
        }
        spawnedLotus.Clear();

        // Si no toca generar flores, terminamos aquí
        if (amountPerBush == 0)
            return;

        // Instanciar UNA flor por cada Bush_03
        foreach (GameObject bush in bushes)
        {
            Vector3 pos = bush.transform.position + Vector3.up * heightOffset;
            GameObject lotus = Instantiate(lotusPrefab, pos, Quaternion.identity);

            // Guardamos la flor para poder destruirla después
            spawnedLotus.Add(lotus);
        }
    }
}

