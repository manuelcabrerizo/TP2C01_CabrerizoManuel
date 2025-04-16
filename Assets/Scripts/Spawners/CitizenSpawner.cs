using System.Collections.Generic;
using UnityEngine;

public class CitizenSpawner : MonoBehaviour
{
    [SerializeField] Citizen citizenPrefab;
    private PoolAllocator<Citizen> citizenPool;
    private List<Citizen> spawnedCitizens;

    private void Awake()
    {
        citizenPool = new PoolAllocator<Citizen>(OnCreatePooledObject,
            OnDestroyPooledObject, OnGetFromPool, OnReleaseToPool);

        spawnedCitizens = new List<Citizen>();
        for(int i = 0; i < 250; ++i)
        {
            spawnedCitizens.Add(citizenPool.Get());
        }
    }

    private Citizen OnCreatePooledObject()
    {
        return Instantiate(citizenPrefab);
    }

    private void OnReleaseToPool(Citizen citizen)
    {
        citizen.gameObject.SetActive(false);
    }

    private void OnGetFromPool(Citizen citizen)
    {
        citizen.gameObject.SetActive(true);
    }

    private void OnDestroyPooledObject(Citizen citizen)
    {
        Destroy(citizen);
    }
}