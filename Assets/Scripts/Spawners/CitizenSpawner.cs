using System.Collections.Generic;
using UnityEngine;

public class CitizenSpawner : MonoBehaviour
{
    [SerializeField] Citizen citizenPrefab;
    [SerializeField] GameObject parent;
    private PoolAllocator<Citizen> citizenPool;
    private List<Citizen> spawnedCitizens;

    private void Start()
    {
        citizenPool = new PoolAllocator<Citizen>(OnCreatePooledObject,
            OnDestroyPooledObject, OnGetFromPool, OnReleaseToPool);
        spawnedCitizens = new List<Citizen>();
        EventManager.Instance.onCitizenRelease.AddListener(OnCitizenRelease);
    }

    public void Clear()
    {
        for(int i = 0; i < spawnedCitizens.Count; ++i)
        {
            citizenPool.Release(spawnedCitizens[i]);
        }
        spawnedCitizens.Clear();
    }

    public void SpawnEnemies(int count)
    {
        for(int i = 0; i < count; ++i)
        {
            spawnedCitizens.Add(citizenPool.Get());
        }
    }

    private void OnCitizenRelease(Citizen citizen)
    {
        spawnedCitizens.Remove(citizen);
        citizenPool.Release(citizen);
    }

    private Citizen OnCreatePooledObject()
    {
        Citizen citizen = Instantiate(citizenPrefab);
        citizen.transform.SetParent(parent.transform);
        return citizen;
    }

    private void OnReleaseToPool(Citizen citizen)
    {
        citizen.OnRelease();
        citizen.gameObject.SetActive(false);
    }

    private void OnGetFromPool(Citizen citizen)
    {
        citizen.gameObject.SetActive(true);
        citizen.OnAquire();
    }

    private void OnDestroyPooledObject(Citizen citizen)
    {
        Destroy(citizen);
    }
}